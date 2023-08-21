<?php
	include "conn.php";
	session_start();
	$time=$_POST['time']; 
	 //$oname=$_SESSION['oname'];
	$cid=$_POST['cid'];
	$mfr=$_POST['mfr'];
	$mod=$_POST['mod'];	
	$mfd=$_POST['mfd'];
	
	$lbl=$_POST['lbl'];
	$lbl = mysqli_real_escape_string($con,$lbl);	
	$sm=$_POST['sm'];	
	$mm=$_POST['mm'];	
	$bf=$_POST['bf'];	
	$reel=$_POST['reel'];	
	$installed=$_POST['installed'];	
	$cabtype=$_POST['cabtype'];	
	$installtyp=$_POST['installtyp'];
	$sheath=$_POST['sheath'];
	$diameter=$_POST['diameter'];
		
	
	
	//$roadway=$_POST['roadway'];	
	
	// $owner=$_SESSION['owner'];
	$owner=$_SESSION['oid'];
	$ownerCD=$_SESSION['OWNER_CD'];
	$jkey=$_SESSION['jobkey'];
	//$tagval=$_POST['tagval'];
	$uid=$_SESSION['uid'];
	if(isset($_SESSION['uid']) && isset($_SESSION['OWNER_CD'])){
	//$owner="demo_owner";
	//Added 1_7_19		
	$job=$_SESSION['jobnum'];
	$stage=$_SESSION['stage'];
	$country=$_SESSION['country'];
	$cablelen=$_POST['cablelen'];
	$ceo=$_POST['ceo'];	
  ////*******Getting owner key******
  $ownerkeysql="select * from `owner` where OWNER_CD='$ownerCD' and record_state='L'";
  $resoquery=mysqli_query($con,$ownerkeysql);
  $resokey=mysqli_fetch_assoc($resoquery);
  $ownerkey=$resokey['key'];
  //
   $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'a_fiber_cable'";
   $autoquery=mysqli_query($con,$autoidquery);
   $keyassoc=mysqli_fetch_assoc($autoquery);
   $autoid=$keyassoc['AUTO_INCREMENT']; 
//************
  $chksql="select * from a_fiber_cable where OWNER_CD='$ownerCD' and record_state='L' and cable_id='$cid'";		
  $chkqry=mysqli_query($con,$chksql);
  if(mysqli_num_rows($chkqry)>0){
  	$fetchfiber=mysqli_fetch_assoc($chkqry);
  	$rid=$fetchfiber['id'];
  	$autoid=$fetchfiber['key'];
  	$updatefiber="update a_fiber_cable set record_state='H',updated_on='$time' where id='$rid'";
  	mysqli_query($con,$updatefiber);
  }
  //***********

	
	 
	$sql="insert into a_fiber_cable(cable_id,manufactured_date,label,SM_COUNT_CD,MM_COUNT_CD,buffer_count,reel_id,YEAR_INSTALLED,OWNER_CD,created_on,created_by,job,stage,COUNTY,LENGTH,GEO_LENGTH,`key`,`cable_type`,owner_key,JOB_KEY,FIBER_MANUF_CD,FIBER_STRAND_COUNT_CD,CONSTRUCTION_TYPE_CD,FIBER_SHEATH_TYPE_CD,a_fiber_reel_key,manufacturer_key,model_key,MM_diameter) values('$cid','$mfd','$lbl','$sm','$mm','$bf','$reel','$installed','$ownerCD','$time','$uid','$job','$stage','$country','$cablelen','$ceo','$autoid','$cabtype','$ownerkey','$jkey','$mfr','$mm','$installtyp','$sheath','$reel','$mfr','$mod','$diameter')";
		// 
		if($_POST['cid']!=""){
		mysqli_query($con,$sql);
		$msg= array("d" => "1");
		echo json_encode($msg);
	}
 } else {
 	$msg= array("d" => "0");
		echo json_encode($msg);

 }

	
	
?>
