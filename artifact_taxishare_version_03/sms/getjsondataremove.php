<?
header("Cache-control: no-cache, must-revalidate");
header("Expires: Mon, 26 Jul 1997 05:00:00 GMT"); // Date in the past

include "../taxishare/connect.php";

$arr = array();
$strQuery = "SELECT DISTINCT upper((CASE location WHEN location THEN location ELSE sms_format END)) AS sms_format " .
            "FROM " .
            "(SELECT DISTINCT (SELECT DISTINCT route_name FROM  msroute a inner join routedetail b ON a.id = b.routeid WHERE b.route_detail_name = sms_format)AS location  " .
            ",sms_format FROM incomingsmslog WHERE ((status_code = 2 or status_code = 3) AND DATE_ADD(date_time, INTERVAL 15 MINUTE)>=NOW()) OR ((status_code = 0 or status_code = 1) AND ((date_time >= date_add(now(), interval -30 MINUTE) AND date_time < date_add(now(), interval -15 MINUTE)))) ORDER BY date_time DESC) P; ";
            //",sms_format FROM incomingsmslog WHERE (status_code = 0 or status_code = 1) AND ((date_time >= date_add(now(), interval -30 MINUTE) AND date_time < date_add(now(), interval -15 MINUTE))) ORDER BY date_time DESC) P; ";
            //",sms_format FROM incomingsmslog WHERE (status_code = 0 or status_code = 1) ORDER BY date_time DESC) P; ";

//echo $strQuery;

$rs = mysql_query($strQuery);

while($obj = mysql_fetch_object($rs)) {
	$arr[] = $obj;
}
echo json_encode($arr);

//$result = mysql_query("update incomingsmslog set status_gmap = 1 WHERE status_gmap = 0");

?>
