<?php
$siteName=$_SERVER['HTTP_HOST'];
ob_start();
 include "../taxishare/connect.php";

 // Initialize Curl_Init sending sms component
 $user_agent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
 $ch = curl_init();
 curl_setopt($ch, CURLOPT_SSL_VERIFYHOST,  2);
 curl_setopt($ch, CURLOPT_USERAGENT, $user_agent);
 curl_setopt($ch, CURLOPT_RETURNTRANSFER,1);
 curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, FALSE);  // this line makes it work under https
 //$urlsmsbullet = "https://www.smsbullet.com.au/msg.php?u=SMS02437&p=CSsms89&d=";

//$urltaxishare = "http://taxi.urvoting.com/sms/incoming.php?to=TO&from=FROM&msg=MSG&date=now()";

 $urltaxishare = "http://localhost/public_html/sms/incoming.php?&date=now()&to=";

 //$urltaxishare = "http://taxi.urvoting.com/sms/incoming.php?&date=now()&to=";

 // End Initialization

  $i=0;
  while($i<25)
  {
     $msg = "campsie";
     $url = $urltaxishare."0447100264&msg=".urlencode($msg)."&from=054222102".$i;
     $i=$i + 1;
     echo $url;
     curl_setopt($ch, CURLOPT_URL,$url);
     $ret=curl_exec ($ch);
  }

  while($i<50)
  {
     $msg = "vineyard";
     $url = $urltaxishare."0447100264&msg=".urlencode($msg)."&from=054222102".$i;
     $i=$i + 1;
     echo $url;
     curl_setopt($ch, CURLOPT_URL,$url);
     $ret=curl_exec ($ch);
  }

  while($i<75)
  {
     $msg = "waterloo";
     $url = $urltaxishare."0447100264&msg=".urlencode($msg)."&from=054222102".$i;
     $i=$i + 1;
     echo $url;
     curl_setopt($ch, CURLOPT_URL,$url);
     $ret=curl_exec ($ch);
  }

  while($i<100)
  {
      $msg = "ryde";
      $url = $urltaxishare."0447100264&msg=".urlencode($msg)."&from=054222102".$i;
      $i=$i + 1;
      echo $url;
      curl_setopt($ch, CURLOPT_URL,$url);
      $ret=curl_exec ($ch);
  }

 echo($ch);

 curl_close ($ch); //ends curl_init
 echo 'OK'; // Send response to SMS provider to confirm the sms has been received OK and no need to re-send
ob_end_flush();
?>