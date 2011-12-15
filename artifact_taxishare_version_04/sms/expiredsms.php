<?php
  // Initialize Curl_Init sending sms component
  $user_agent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
  $ch = curl_init();
  curl_setopt($ch, CURLOPT_SSL_VERIFYHOST,  2);
  curl_setopt($ch, CURLOPT_USERAGENT, $user_agent);
  curl_setopt($ch, CURLOPT_RETURNTRANSFER,1);
  curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);  // this line makes it work under https
  $urlsmsbullet = "https://www.smsbullet.com.au/msg.php?u=SMS02437&p=CSsms89&d=";
  // End Initialization

      try
      {
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

        $getunmatched = mysql_query("SELECT DISTINCT id, phone_number FROM incomingsmslog WHERE status_code = 0 AND DATE_ADD(date_time, INTERVAL 20 MINUTE)<NOW() ORDER BY date_time DESC");
        $msg = "Your TaxiShare message has timed out, if you wish to rejoin the queue the please send another sms.";
	 $dt = date("Y-m-d H:i:s");
        while($rs=mysql_fetch_array($getunmatched))
        {
          $foundid = $rs['id'];
          $foundphonenumber = $rs['phone_number'];
          $url = $urlsmsbullet.urlencode($foundphonenumber)."&m=".urlencode($msg)."&o=TaxiShare";
          curl_setopt($ch, CURLOPT_URL,$url);
          //$ret=curl_exec ($ch); // OPEN THIS FOR LIVE VERSION -- 1 OF 1
          //$result = mysql_query("INSERT INTO outgoingsmslog(phone_number,sms_format,date_time) VALUES ('$foundphonenumber','$msg','$dt')");
          $result = mysql_query("UPDATE incomingsmslog SET status_code = 2 WHERE id = '$foundid'");
        }

        $getunmatchedtwitter = mysql_query("SELECT DISTINCT id, twitter_account FROM incomingtwitterlog WHERE status_code = 0 AND DATE_ADD(date_time, INTERVAL 15 MINUTE)<NOW() ORDER BY date_time DESC");
        $msg = "Your TaxiShare message has timed out, if you wish to rejoin the queue the please send another message.";
        $dt = date("Y-m-d H:i:s");
        while($rstwitter=mysql_fetch_array($getunmatchedtwitter))
        {
          $id = $rstwitter['id'];
          $foundtwitteraccount = $rstwitter['twitter_account'];
          $result = mysql_query("INSERT INTO outgoingtwitterlog(twitter_account,twitter_text,date_time) VALUES ('$foundtwitteraccount','$msg','$dt')");
          $result = mysql_query("UPDATE incomingtwitterlog SET status_code = 2 WHERE id = '$id'");
        }

        mysql_close($connect);
	 curl_close ($ch); //ends curl_init
        echo 'OK'; // Send response to SMS provider to confirm the sms has been received OK and no need to re-send
      }//end try
      catch (Exception $e)
      {
        $errorMsgs = $e->getTrace();
        echo $errorMsgs;
      }

?>
