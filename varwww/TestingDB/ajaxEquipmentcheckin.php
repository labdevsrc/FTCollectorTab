<?php
	
	include "conn.php";
	session_start();
	$uid=$_SESSION['uid']; 
	$longitude=$_SESSION['lo'];
    $lattitude=$_SESSION['la'];
    $hrs=$_POST['hrs'];
    $currenttime=$_POST['time'];
    $jno=$_SESSION['jobnum'];
	$sql="insert into job_events (job, employee, leader, event_time, event_type, latitude, longitude, miles_hours) values('$jno','$uid','L','$currenttime','4','$lattitude','$longitude','$hrs')";
	mysqli_query($con,$sql);
	$_SESSION['equip']="1";
	$sql1="select id  from job_events where event_time='$currenttime'";
	$res=mysqli_query($con,$sql1);
	$rows=mysqli_fetch_assoc($res);
	$_SESSION['equiprowid']=$rows['id'];
	echo "Equipment Checkin success. Now please give your signature.";

?>