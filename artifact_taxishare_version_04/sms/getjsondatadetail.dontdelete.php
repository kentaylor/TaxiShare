<?
header("Cache-control: no-cache, must-revalidate");
header("Expires: Mon, 26 Jul 1997 05:00:00 GMT"); // Date in the past

include "../taxishare/connect.php";

$arr = array();
$strQuery = "SELECT upper(Q.contact_name) as contact_name, P.id, P.phone_number,upper((CASE location WHEN location THEN location ELSE sms_format END)) AS sms_format,date_time,status_code " .
            "FROM " .
            "(SELECT id, phone_number,(SELECT DISTINCT route_name FROM  msroute a inner join routedetail b ON a.id = b.routeid WHERE b.route_detail_name = sms_format)AS location  " .
            ",sms_format, date_time,status_code FROM incomingsmslog WHERE (status_code = 0 or status_code = 1) AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW() ORDER BY sms_format, id DESC) P LEFT JOIN contactnumber Q on P.phone_number = Q.phone_number ";
            //",sms_format, date_time,status_code FROM incomingsmslog WHERE (status_code = 0 or status_code = 1) ORDER BY sms_format, id DESC) P LEFT JOIN contactnumber Q on P.phone_number = Q.phone_number ";

//echo $strQuery;

$rs = mysql_query($strQuery);

while($obj = mysql_fetch_object($rs)) {
	$arr[] = $obj;
}
echo json_encode($arr);

//$result = mysql_query("update incomingsmslog set status_gmap = 1 WHERE status_gmap = 0");

?>
