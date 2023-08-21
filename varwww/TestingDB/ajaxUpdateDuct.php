<?php
	session_start();
	include "conn.php";
	 $time=$_POST['time']; 
	$owner=$_POST['owner'];	
	$ownerCD=$_POST['OWNER_CD'];
	$tag=$_POST["tag"];
	$type1='10';	
/*if($_SESSION['sitename']=="building") {
    $htype="1";
  }else if($_SESSION['sitename']=="cabinet") {
      $htype="2";
  }else if($_SESSION['sitename']=="structure") {
      $htype="3";
  }else if($_SESSION['sitename']=="pull box") {
      $htype="4";
  }*/
 $htype=$_POST['site_type_key'];
	$uid=$_POST['uid'];
	//$time='now()';	

	$sql7="select * from CONDUITS_GROUP where host_tag_number='$tag' and host_type='$htype' and created_by='$uid' and OWNER_CD='$ownerCD' and record_state='L'";
		$msg1= array("d" => "0");
		$qr=mysqli_query($con,$sql7);
		$res=mysqli_fetch_array($qr);
		if(mysqli_num_rows($qr) > 0 ){
			$msg= array("d" => "1","direction" => $res['direction'],"direction_count" => $res['direction_count'],"DUCT_SIZE_CD" => $res['DUCT_SIZE_CD'],"color" => $res['color'],"CONDUIT_GROUP_ID" => $res['CONDUIT_GROUP_ID'],"DUCT_TYPE_CD" => $res['DUCT_TYPE_CD'],"USAGE_CD" => $res['USAGE_CD'],"DUCT_INSTALLATION_CD" => $res['DUCT_INSTALLATION_CD'],"has_seal" => $res['has_seal'],"in_use" => $res['in_use'],"has_trace_wire" => $res['has_trace_wire'],"has_pull_tape" => $res['has_pull_tape'],"in_use_percent" => $res['in_use_percent']
			);
			echo json_encode($msg);
		}else{
			echo json_encode($msg1);
		}		
	
?>

