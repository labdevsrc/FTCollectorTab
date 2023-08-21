

	<?php
	session_start();
	include "conn.php";		
	//$mac=$_SESSION['uid'];
  $time=$_POST['time'];
	$longitude1=$_POST['longitude'];
  $latitude1=$_POST['lattitude'];
  $altitude1=$_POST['altitude'];
  $lpno=$_POST['locate_point_number'];
// {"lt": longitude,"ltti": latitude,"alt": altitude,"lpn": lpn,"fromtag1": //fromtag,"fromduct1": fromduct,"totag1": totag,"toduct1": toduct};

   $tagfrom=$_POST['fromtag1'];
   $ductfrom=$_POST['fromduct1'];
   $tagto=$_POST['totag1'];
   $ductto=$_POST['toduct1'];
   //$time=$time;
   $jno=$_POST['jobnum'];
   $uid=$_POST['uid'];
  
if(isset($_POST['uid']) && trim($uid)!=""){
  $sql="insert into gps_point (locate_point_number,longitude,lattitude,altitude,tag_from,duct_from,tag_to,duct_to,user_id,time,created_on,created_by,job_number) values('$lpno','$longitude1','$latitude1','$altitude1','$tagfrom','$ductfrom','$tagto','$ductto','$uid','$time','$time','$uid','$jno')";
    $res=mysqli_query($con,$sql);
    $msg= array("d" => "Record GPS for this site has been sucessfully...");   
} else {
  $msg= array("d" => "0");
}

//echo "Save Sucessfully...";

echo json_encode($msg);


	
?>