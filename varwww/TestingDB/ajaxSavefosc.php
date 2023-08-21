<?php
	include "conn.php";
	session_start();
	$time=$_POST['time'];
	$cid=$_POST['cable_id'];
	$oname=$_POST['oname'];
	$owner1=$_POST['oid'];
		
	//{"mfr": mfr,"lbl" : lbl,"sm" : sm,"mm" : mm,"bf" : bf,"she" : she,"mfd" : mfd,"reel" : reel};
	$mfr=$_POST['manufacturer'];
	$mod=$_POST['model'];
	
	$lbl=$_POST['label'];
	$sm=$_POST['singlemode_count'];
	$mm=$_POST['multimode_count'];
	$bf=$_POST['buffer_count'];
	$she=$_POST['sheath'];
	$mfd=$_POST['manufactured_date'];
	$reel=$_POST['reel'];
	$owner=$_POST['owner'];
	$uid=$_POST['uid'];
	$ownerCD=$_POST['OWNER_CD'];
	

	$asite=$_POST['asite'];
	$zsite=$_POST['zsite'];
	$installedDate=$_POST['installed_date'];
	$geo=$_POST['geo_length'];
	$country=$_POST['country'];
	
	//$owner="demo_owner";
	//$sql="insert into CABLES_FIBER_OPTIC (OWNER_CD,cable_id,FIBER_MANUF_CD,label,FIBER_SHEATH_TYPE_CD,SM_MM_SEQ_CD,SM_COUNT_CD,MM_COUNT_CD,manufactured_date,reel_id,created_on,created_by,job,A_side_tag_number,Z_side_tag_number,YEAR_INSTALLED,GEO_LENGTH,COUNTY) values('$ownerCD','$cid','$mfr','$lbl','$she','$bf','$sm','$mm','$mfd','$reel',now(),'$uid','$oname','$asite','$zsite','$installedDate','$geo','$country')";


	// $sql="insert into CABLES_FIBER_OPTIC (OWNER_CD,cable_id,FIBER_MANUF_CD,label,FIBER_SHEATH_TYPE_CD,sm_buffers,mm_buffers,manufactured_date,reel_id,created_on,created_by,job,GEO_LENGTH,COUNTY,manufacturer,model) values('$ownerCD','$cid','$mfr','$lbl','$she','$sm','$mm','$mfd','$reel','$time','$uid','$oname','$geo','$country','$mfr','$mod')";

	$sql="insert into a_fiber_cable (OWNER_CD,cable_id,FIBER_MANUF_CD,label,FIBER_SHEATH_TYPE_CD,manufactured_date,reel_id,created_on,created_by,job,GEO_LENGTH,COUNTY) values('$ownerCD','$cid','$mfr','$lbl','$she','$mfd','$reel','$time','$uid','$oname','$geo','$country')";

	//$sql="insert into a_fiber_cable (cable_id) values('$cid')";
	//sm_buffers manufacturer
	$sql1="select cable_id from a_fiber_cable where cable_id='$cid'";
	$res=mysqli_query($con,$sql1);	
	if(mysqli_num_rows($res) > 0){		
		$msg= array("d" => "Not saved...");
		echo json_encode($msg);
	}else{
		$res1=mysqli_query($con,$sql);
			if($res1){
			$msg= array("d" => "Sucessfully saved...");
			} else{
				$msg= array("d" => "Was not saved Sucessfully...");
			}
			echo json_encode($msg);
	}

	
	
?>

