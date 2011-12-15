<?php
 //include "../taxishare/connect.php";

 $host="localhost";
 $user="root";
 $pass="root";
 $db="taxishare";

 $connect=mysql_connect($host,$user,$pass);
 if(!$connect)
 {
   echo"Connection Failed";
 }

 $database=mysql_select_db($db);

 $t= new twitter();  

 $sqlgetmax = "SELECT MAX(twitter_id) FROM incomingtwitterlog";
 $getmax = mysql_query($sqlgetmax);

 $res = $t->getTwitterData($getmax);

 if($res===false){
   echo "ERROR<hr/>";
     echo "<pre>";
     echo "</pre>";
 }else{
   echo "SUCCESS<hr/>";
    echo "<pre>";
    //AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY date_time DESC";
        
    $destinationtmp="";
    $destination="";
    $sender="";

    foreach( $res as $element )
        {
            //$sender = $element ->user ->name;
            $sender = $element ->user ->screen_name;
	     $destinationtmp = $element ->text;
            $destination = substr($destinationtmp, 9);

            $twitterid = $element ->id;

            $dt = date("Y-m-d H:i:s");

	     $sqlexisting = "SELECT COUNT(id) FROM incomingtwitterlog WHERE twitter_id ='$twitterid'";
            $recexisting = mysql_query($sqlexisting);
            $countexisting = mysql_result($recexisting,0);	

            if ($countexisting == 0 AND trim($destination) == "cancel")
            {
              $query="SELECT COUNT(id) FROM incomingtwitterlog WHERE status_code = 0 AND twitter_account = '$sender'";
              $reccount = mysql_query($query);               
              $result = mysql_result($reccount,0);		

              if($result > 0)
              {
                 $sqlsearch="SELECT twitter_text FROM incomingtwitterlog WHERE status_code = 0 AND twitter_account = '$sender'";
                 $datasearch = mysql_query($sqlsearch); 

                 $arr = mysql_fetch_array($datasearch);
	          $text = $arr['twitter_text'];

		   $result = mysql_query("INSERT INTO incomingtwitterlog (twitter_account,twitter_text,date_time,twitter_id,status_code) VALUES ('$sender','$destination','$dt','$twitterid',0)");
                
		   $result = mysql_query("UPDATE incomingtwitterlog SET  status_code = 3 WHERE status_code = 0 AND twitter_account = '$sender'");
                 if (!$result)
                 {
                   die('Invalid query: ' . mysql_error());
                 }

                 $msg = "Your TaxiShare message '".$text."' has been Cancelled. Thank you for using Taxishare.";
                 $res = $t->update("@".$sender." ".$msg);
 	
                 $result = mysql_query("INSERT INTO outgoingtwitterlog(twitter_account,twitter_text,date_time) VALUES ('$sender','$destination','$createddate')");

               }
               exit();
            }

            if($countexisting == 0 AND $destination != "")
            {
               
              //Check whether the same twitter no has send a message before, incase there is an unmatched one set the status to cancel.
              $sqlexisting = "SELECT COUNT(id) FROM incomingtwitterlog WHERE status_code = 0 AND twitter_account = '$sender' AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY date_time DESC";
              $recexisting = mysql_query($sqlexisting);
              $countexisting = mysql_result($recexisting,0);
              if($countexisting > 0)
              {
                $result = mysql_query("UPDATE incomingtwitterlog SET  status_code = 3 WHERE status_code = 0 AND twitter_account = '$sender'");
              }
              // End check whether the same twitter no has send a nessage before

               $result = mysql_query("INSERT INTO incomingtwitterlog (twitter_account,twitter_text,date_time,twitter_id,status_code) VALUES ('$sender','$destination','$dt','$twitterid',0)");
               if (!$result)
               {
                  die('Invalid query: ' . mysql_error());
               }

                // Firstly search from incomingtwitterlog
                $sqlfind = "SELECT DISTINCT twitter_account, twitter_text FROM incomingtwitterlog WHERE (status_code = 0) AND SOUNDEX(twitter_text) = SOUNDEX('$destination') AND twitter_account <> '$sender' AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY date_time DESC LIMIT 2 ";
                $gettwittertext = mysql_query($sqlfind);

                 if (!$gettwittertext )
                 {
                   die('Invalid query: ' . mysql_error());
                 }
                 $arrsmsformat = mysql_fetch_array($gettwittertext);
                 $foundtwitteraccount = $arrsmsformat['twitter_account'];
                 //$firsttwitteraccount = $foundtwitteraccount;

                 $getsms = mysql_query($sqlfind);

                 if ($foundtwitteraccount !="")
                 {
                    $msgfirst = "";
                    $msgsecond = "";

                    $msgsecond = stripslashes(urldecode($sender));

                    $i=0;
                    while($rs=mysql_fetch_array($getsms))
                    {
                        $foundtwitteraccount = $rs['twitter_account'];
                        $foundtwittertext = $rs['twitter_text'];
                        $msgfirst = $msgfirst." and @".stripslashes(urldecode($foundtwitteraccount));
                        $i=$i+1;
                    }

                    $msgfirst = $msgfirst." [TOBE] also going to ".stripslashes(urldecode($destination)).", please contact them.";
                    $msgfirst = substr($msgfirst,4);
                    if ($i > 1)
                      $msgfirst = str_replace("[TOBE]","are",$msgfirst);
                    else
                      $msgfirst = str_replace("[TOBE]","is",$msgfirst);

                    $msgfirst = trim($msgfirst);
                    $res = $t->update("@".$sender." ".$msgfirst);

	             $result = mysql_query("INSERT INTO outgoingtwitterlog(twitter_account,twitter_text,date_time) VALUES ('$sender','$msgfirst','$dt')");

                    $msgsecond = "@".stripslashes(urldecode($sender))." is also going to ".stripslashes(urldecode($destination)).", they have been asked to contact you.";
                    $msgsecond = trim($msgsecond);
                    $res = $t->update("@".$foundtwitteraccount." ".$msgsecond);

                    $result = mysql_query("INSERT INTO outgoingtwitterlog(twitter_account,twitter_text,date_time) VALUES ('$foundtwitteraccount','$msgsecond','$dt')");

                    $result = mysql_query("UPDATE incomingtwitterlog SET status_code = 1 WHERE twitter_account = '$foundtwitteraccount' AND twitter_text =  '$foundtwittertext'");
                    $result = mysql_query("UPDATE incomingtwitterlog SET status_code = 1 WHERE twitter_account = '$sender' AND twitter_text =  '$destination'");
                 }
                 else
                 {
                    $msg = "There are currently no matches with ".$destination.", we will let you know if one arrives.";
                    $res = $t->update('@'.$sender.' '.$msg);
                    $result = mysql_query("INSERT INTO outgoingtwitterlog (twitter_account,twitter_text,date_time) VALUES ('$sender','$msg','$dt')");
                 }
            }

       }

    echo "</pre>";
 }

//////////////////////////////////////////

class twitter{
    var $username='taxi@urvoting.com';
    var $password='420yaj';
    var $user_agent='';
    var $headers=array('X-Twitter-Client: ',
                                            'X-Twitter-Client-Version: ',
                                            'X-Twitter-Client-URL: ');

    var $responseInfo=array();

    function twitter(){}

    function update($status){
        $request = 'http://twitter.com/statuses/update.xml';
        $postargs = 'status='.urlencode($status);
	 $res = $this->process($request,$postargs);
	return $res;
    }

     function getTwitterData($sinceid){
        $qs='?since_id='.intval($sinceid);
        $request = 'http://twitter.com/statuses/mentions.xml'.$qs;
        return $this->process($request);
    }

    function process($url,$postargs=false){

        $ch = curl_init($url);

        if($postargs !== false){
	     curl_setopt($ch, CURLOPT_POST, 1);
            curl_setopt($ch, CURLOPT_POSTFIELDS, $postargs);
        }

        if($this->username !== false && $this->password !== false)
            curl_setopt($ch, CURLOPT_USERPWD, $this->username.':'.$this->password);

        curl_setopt($ch, CURLOPT_TIMEOUT, 0);
        curl_setopt($ch, CURLOPT_VERBOSE, 1);
        curl_setopt($ch, CURLOPT_HEADER, 0);
        curl_setopt($ch, CURLOPT_USERAGENT, $this->user_agent);
        curl_setopt($ch, CURLOPT_FOLLOWLOCATION,1);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
        
        $response = curl_exec($ch);

        $this->responseInfo=curl_getinfo($ch);
        curl_close($ch);
        if(intval($this->responseInfo['http_code'])==200){
            if(class_exists('SimpleXMLElement')){
                $xml = new SimpleXMLElement($response);
                return $xml;
            }else{
                return $response;
            }
        }else{
            return false;
        }
    }
}
?>
