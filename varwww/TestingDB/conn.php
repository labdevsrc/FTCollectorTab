<?php
 header('Cache-Control: no cache');
 session_cache_limiter('private_no_expire'); 

 ini_set('session.gc_maxlifetime', 172800);
 
$lifetime=600;
session_start();
setcookie(session_name(),session_id(),time()+$lifetime);
	$server="myfibertrak.crrkc3tsjnuq.us-east-2.rds.amazonaws.com";
	$username="fibertrakadmin";//  "Rajib.Banerjee";
	$pass="pG6KJuSDm1so";// "95Rbsps5mvZ9T8Wm";
	$database= "Testing"; 
	$con=mysqli_connect($server,$username,$pass,$database);
	//echo "Connected";
	if (!$con) {
    die("Database Connection failed: " . mysqli_connect_error());
    }
    
    $GLOBALS['config'] = $con;
    $GLOBALS['test'] = "1233";
    $GLOBALS['createdfrm']="field collection";
	$GLOBALS['appversion']="Version 4.0";

	function getSiteId(){
		$_SESSION['siteid']="123";
  		$sqlsiteid="select max(id) sid from Site";
  		$resid=mysqli_query($con,$sqlsiteid);
  		$geSiteid=mysqli_fetch_assoc($resid);
  		$_SESSION['siteid']='i-'.$geSiteid['sid'];
     }
    
  //    function getSiteId($model){
  //    		//include "conn.php";
		// $sql="select * from model where number='$model'";
		// $qr=mysqli_query($con,$sql);
		// $res=mysqli_fetch_assoc($qr);
		// //return $res;
		// echo json_encode($res);
  //    }

//Record Log
$file_name = 'pagelog.txt';
$myfile = fopen($file_name, 'a');// or die('Cannot open file: '.$file_name); 
if(isset($_SESSION['uid'])){

$actual_link = "\n".(isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? "https" : "http") . "://{$_SERVER['HTTP_HOST']}{$_SERVER['REQUEST_URI']}"."   ".date('Y-m-d h:i:s')." UID: ".$_SESSION['uid'];
} else{
	$actual_link = "\n".(isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? "https" : "http") . "://{$_SERVER['HTTP_HOST']}{$_SERVER['REQUEST_URI']}"."   ".date('Y-m-d h:i:s');
}
fwrite($myfile, $actual_link);
fclose($myfile);
?>
