<?php
$siteName=$_SERVER['HTTP_HOST'];
ob_start();
 include "../taxishare/connect.php";
 // Section HTTP REQUEST
 if (isset($_REQUEST['from']))
 {
    $phone_number =stripslashes(urldecode($_REQUEST['from']));
    $phone_number= trim(str_replace("\"", "", $phone_number));
    if($phone_number == "Unknown <Unknown>")
       $phone_number='Unknown';  

    //echo $phone_number;
 }
 $to =$_REQUEST['to'];
 $msg =trim($_REQUEST['msg']);
 $dt =$_REQUEST['date'];
 $dt = date("Y-m-d H:i:s");
 // End Section HTTP REQUEST

 // Initialize Curl_Init sending sms component
 $user_agent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
 $ch = curl_init();
 curl_setopt($ch, CURLOPT_SSL_VERIFYHOST,  2);
 curl_setopt($ch, CURLOPT_USERAGENT, $user_agent);
 curl_setopt($ch, CURLOPT_RETURNTRANSFER,1);
 curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);  // this line makes it work under https
 $urlsmsbullet = "https://www.smsbullet.com.au/msg.php?u=SMS02437&p=CSsms89&d=";
 // End Initialization
 
 // Constanta declaration
 define("MAX_SMS_PERQUARTER", 10);
 define("MAX_SMS_PERHOUR", 10);
 define("MAX_SMS_PERDAY", 20);
 // End Constanta

 //validation rule 01: no more than 5 sms can be sent to one number in 15 minutes, no more than 10 in an hour and no more than 20 in one day
 //validation rule 02: no more than 10 sms can be sent to one number in 15 minutes, no more than 10 in an hour and no more than 20 in one day
 function ValidToSend($phonenumber)
 {
    $query="SELECT COUNT(phone_number) FROM outgoingsmslog WHERE phone_number='$phonenumber' AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY date_time DESC";
    $reccount = mysql_query($query);
    $result = mysql_result($reccount,0);

    if($result > MAX_SMS_PERQUARTER)
    {
      //echo "No more than ".MAX_SMS_PERQUARTER." can be sent to one number in 15 minutes.";
      return false;
    }

    $query="SELECT COUNT(phone_number) FROM outgoingsmslog WHERE phone_number='$phonenumber' AND DATE_ADD(date_time, INTERVAL 60 MINUTE)>=NOW() ORDER BY date_time DESC";
    $reccount = mysql_query($query);
    $result = mysql_result($reccount,0);

    if($result > MAX_SMS_PERHOUR)
    {
      //echo "No more than ".MAX_SMS_PERHOUR." can be sent to one number in 1 hour.";
      return false;
    }

    $query="SELECT COUNT(phone_number) FROM outgoingsmslog WHERE phone_number='$phonenumber' AND DATE_ADD(date_time, INTERVAL 24 HOUR)>=NOW() ORDER BY date_time DESC";
    $reccount = mysql_query($query);
    $result = mysql_result($reccount,0);
    if($result > MAX_SMS_PERDAY)
    {
      //echo "No more than ".MAX_SMS_PERDAY." can be sent to one number in 1 day.";
      return false;
    }
    else
    {
      return true;
    }
 }

 function getcontactname($phone_number)
 {
   $sqlfind = "SELECT DISTINCT contact_name FROM contactnumber WHERE phone_number LIKE '%$phone_number%'";
   $getsmsformat = mysql_query($sqlfind);
   $arrsmsformat = mysql_fetch_array($getsmsformat);
   return $arrsmsformat['contact_name'];
 }

 if (!ValidToSend($phone_number))
 {
   exit();
 }

/*
 $result = mysql_query("INSERT INTO incomingsmslog (phone_number,sms_format,date_time,status_code) VALUES ('$phone_number','$msg','$dt',0)");
 if (!$result)
 {
   die('Invalid query: ' . mysql_error());
 }
*/

 //Check whether it Cancellation type SMS
 if (trim($msg) == "cancel" OR strtoupper(trim($msg)) == "CALL")
 {
    $query="SELECT COUNT(phone_number) FROM incomingsmslog WHERE status_code = 0 AND phone_number = '$phone_number'";
    $reccount = mysql_query($query);
    $result = mysql_result($reccount,0);
    if($result > 0)
    {
       $result = mysql_query("UPDATE incomingsmslog SET  status_code = 3 WHERE status_code = 0 AND phone_number = '$phone_number'");
       if (!$result)
       {
         die('Invalid query: ' . mysql_error());
       }
       $msg = "Your TaxiShare message has been Cancelled. Thank you for using Taxishare.";
       $url = $urlsmsbullet.urlencode($phone_number)."&m=".urlencode($msg)."&o=0447100264";
       curl_setopt($ch, CURLOPT_URL,$url);
       $ret=curl_exec ($ch);  // OPEN THIS FOR LIVE VERSION - 1 OF 4
       $result = mysql_query("INSERT INTO outgoingsmslog(phone_number,sms_format,date_time) VALUES ('$phone_number','$msg','$dt')");
       //echo $url;
     }  
     exit();
 }

 //Ignore when the same phonenumber AND same destination coming
    $sqlexisting = "SELECT COUNT(phone_number) FROM incomingsmslog WHERE sms_format ='$msg' AND status_code = 0 AND phone_number = '$phone_number' AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY date_time DESC";
    $recexisting = mysql_query($sqlexisting);
    $countexisting = mysql_result($recexisting,0);
    if($countexisting > 0)
    {
      exit();
    }
 // End when the same phonenumber AND same destination coming

 //Check whether the same phonenumber has send an sms before, incase there is an unmatched one set the status to cancel.
    $sqlexisting = "SELECT COUNT(phone_number) FROM incomingsmslog WHERE status_code = 0 AND phone_number = '$phone_number' AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY date_time DESC";
    $recexisting = mysql_query($sqlexisting);
    $countexisting = mysql_result($recexisting,0);
    if($countexisting > 0)
    {
      $result = mysql_query("UPDATE incomingsmslog SET  status_code = 3 WHERE status_code = 0 AND phone_number = '$phone_number'");
    }
 // End check whether the same phonenumber has send an sms before

 $result = mysql_query("INSERT INTO incomingsmslog (phone_number,sms_format,date_time,status_code) VALUES ('$phone_number','$msg','$dt',0)");
 if (!$result)
 {
   die('Invalid query: ' . mysql_error());
 }

 // Find matched sms within 15 minutes for 4 people
 //status_code 0=Open, 1=Matched, 2=UnMatched, 3=Cancel
 // $sqlfind = "SELECT DISTINCT phone_number, sms_format FROM incomingsmslog WHERE (status_code = 0 or status_code = 1) AND sms_format LIKE'%$msg%' AND phone_number <> '$phone_number' AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY date_time DESC LIMIT 4 ";

 // Firstly search from incomingsmslog
 $sqlfind = "SELECT DISTINCT phone_number, sms_format FROM incomingsmslog WHERE (status_code = 0) AND SOUNDEX(sms_format) = SOUNDEX('$msg') AND phone_number <> '$phone_number' AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY date_time DESC LIMIT 2 ";
 $getsmsformat = mysql_query($sqlfind);
 if (!$getsmsformat )
 {
   die('Invalid query: ' . mysql_error());
 }
 $arrsmsformat = mysql_fetch_array($getsmsformat);
 $foundphonenumber = $arrsmsformat['phone_number'];
 $firstfoundphonenumber = $foundphonenumber;
 $getsms = mysql_query($sqlfind); // refer to incomingsmslog first

 // Secondly search from routedetail
 if ($foundphonenumber == "")
 {
    $sqlfindroute = "SELECT phone_number, sms_format from incomingsmslog where (status_code = 0)  AND phone_number <> '$phone_number' AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() AND sms_format in (SELECT route_name FROM msroute a INNER JOIN routedetail b ON a.id = b.routeid WHERE SOUNDEX(route_detail_name) = SOUNDEX('$msg')) ORDER BY date_time DESC LIMIT 2";
    $getroute = mysql_query($sqlfindroute);
    if (!$getroute)
    {
      die('Invalid query: ' . mysql_error());
    }

    $arrroute = mysql_fetch_array($getroute);
    $foundphonenumber = $arrroute['phone_number'];
    $firstfoundphonenumber = $foundphonenumber;
    $getsms = mysql_query($sqlfindroute); // refer to route data incase no found in incomingsmslog
 }

 if ($foundphonenumber !="")
 {
    $msgsecond = stripslashes(urldecode($phone_number));
    $i=0;
    while($rs=mysql_fetch_array($getsms))
    {
        $contactname ="";
        $foundphonenumber = $rs['phone_number'];
	 //$foundphonenumber = "+".$foundphonenumber;
        $founddsmsformat = $rs['sms_format'];
        $contactname  =  getcontactname($foundphonenumber);
        $msgfirst = $msgfirst." and ".$contactname." +".stripslashes(urldecode($foundphonenumber));
        $i=$i+1;
    } // end while

    $msgfirst = $msgfirst." [TOBE] also going to ".stripslashes(urldecode($founddsmsformat)).", please call their number.";
    $msgfirst = substr($msgfirst,4);
   
    if ($i > 1)
      $msgfirst = str_replace("[TOBE]","are",$msgfirst);
    else
      $msgfirst = str_replace("[TOBE]","is",$msgfirst);  

    $msgfirst = trim($msgfirst);
    
    //$url = $urlsmsbullet.urlencode($phone_number)."&m=".urlencode($msgfirst)."&o=".stripslashes(urldecode($firstfoundphonenumber));//  --> send to second person e.g. Jim
    $url = $urlsmsbullet.urlencode($phone_number)."&m=".urlencode($msgfirst)."&o=0447100264";//  --> send to second person e.g. Jim
    
    curl_setopt($ch, CURLOPT_URL,$url);
    $ret=curl_exec ($ch); // OPEN THIS FOR LIVE VERSION - 2 OF 4
    mysql_query("INSERT INTO outgoingsmslog(phone_number,sms_format,date_time) VALUES ('$phone_number','$msgfirst','$dt')");

    $contactname  =  getcontactname($phone_number)." ";
    $msgsecond = "+".stripslashes(urldecode($phone_number))." is also going to ".stripslashes(urldecode($msg)).", they have been asked to call you.";
    $msgsecond = trim($msgsecond);

    //$url = $urlsmsbullet.urlencode($firstfoundphonenumber)."&m=".urlencode($msgsecond)."&o=".stripslashes(urldecode($phone_number));// --> send to first person e.g. Michael
    $url = $urlsmsbullet.urlencode($firstfoundphonenumber)."&m=".urlencode($msgsecond)."&o=0447100264";// --> send to first person e.g. Michael
    
    curl_setopt($ch, CURLOPT_URL,$url);
    $ret=curl_exec($ch); // OPEN THIS FOR LIVE VERSION - 3 OF 4
    $result = mysql_query("INSERT INTO outgoingsmslog(phone_number,sms_format,date_time) VALUES ('$foundphonenumber','$msgsecond','$dt')");
    $result = mysql_query("UPDATE incomingsmslog SET status_code = 1 WHERE phone_number = '$foundphonenumber' AND sms_format =  '$founddsmsformat'");
    $result = mysql_query("UPDATE incomingsmslog SET status_code = 1 WHERE phone_number = '$phone_number' AND sms_format =  '$msg'");

    echo ("Found ".$foundphonenumber);
    echo ("Phone ".$phone_number);
    echo ("Msg ".$msg);

    echo $url;
 }
 else
 {
    $msg = "There are currently no matches with ".$msg.", we will let you know if one arrives.";
    $url = $urlsmsbullet.urlencode($phone_number)."&m=".urlencode($msg)."&o=0447100264";
    curl_setopt($ch, CURLOPT_URL,$url);
    $ret=curl_exec($ch); // OPEN THIS FOR LIVE VERSION - 4 OF 4
    mysql_query("INSERT INTO outgoingsmslog (phone_number,sms_format,date_time) VALUES ('$phone_number','$msg','$dt')");
    //echo $url;
 }

 curl_close ($ch); //ends curl_init
 echo 'OK'; // Send response to SMS provider to confirm the sms has been received OK and no need to re-send
ob_end_flush();
?>