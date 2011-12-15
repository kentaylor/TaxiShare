<?php
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
?>