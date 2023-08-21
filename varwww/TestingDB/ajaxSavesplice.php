  <?php
  session_start();
// {"htag":htag,"mfr":mfr,"mod":mod,"pos":pos,"rack":rack};
  include "conn.php"; 
  include "allfunctions.php"; 
  $time=$_POST['time']; 
  $type=$_POST['type'];
  $fromcable=$_POST['fromcable'];
  $tocable=$_POST['tocable'];
  $frombuffer=$_POST['frombuffer'];
  $tobuffer=$_POST['tobuffer'];
  $fromfiber=$_POST['fromfiber'];
  $tofiber=$_POST['tofiber'];
  $splicetype=$_POST['splicetype']; 
  $site=$_POST['site']; 

  $fcablesh=$_POST['fcablesh'];
  $fcableshe=$_POST['fcableshe']; 
  $tcablesh=$_POST['tcablesh'];
  $tcableshe=$_POST['tcableshe'];
  $tchsh=$_POST['tchsh'];
  $tchshe=$_POST['tchshe'];
  // 
  

  $createdon=$time;

  $Tray=$_POST['Tray'];
  $Slot=$_POST['Slot'];

  $owner=$_POST['oid'];         //$_SESSION['oid'];
  $ownerCD= $_POST['OWNER_CD']; //$_SESSION['OWNER_CD'];
  $createdby= $_POST['uid'];    //$_SESSION['uid'];
  $jno=$_POST['jobnum'];        //$_SESSION['jobnum'];
  $stage= $_POST['stage'];      //$_SESSION['stage'];
  $jobkey=$_POST['jobkey'];     //$_SESSION['jobkey'];

  $ownerky=getOwnerkey($ownerCD);
  $hostky=$site; //getHostkey($site);
  //10    
  $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'a_fiber_connection'";
        $autoquery=mysqli_query($con,$autoidquery);
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT'];

if(isset($_POST['fromcable']) && isset($_SESSION['OWNER_CD']) && isset($_SESSION['oid'])){
	if($splicetype == "1"){
     $frombuffer=getColor($frombuffer);
     $tobuffer=getColor($tobuffer);
     $fromfiber=getColor($fromfiber);
     $tofiber=getColor($tofiber);
  $sql="insert into a_fiber_connection(`key`,OWNER_CD,created_on,created_by,type,from_cable_id,to_cable_id,from_buffer_color,to_buffer_color,from_fiber_color,to_fiber_color,to_blade_number,to_port_number,job,stage,host_tag,job_key,owner_key,host_site_key,from_cable_sheath_mark, from_cable_sheath_mark_eom, to_cable_sheath_mark,to_cable_sheath_mark_uom,to_chassis_sheath_mark, to_chassis_sheath_mar_uom) values('$autoid','$ownerCD','$createdon','$createdby','S','$fromcable','$tocable','$frombuffer','$tobuffer','$fromfiber','$tofiber','$Tray','$Slot','$jno','$stage','$site','$jobkey','$ownerky','$hostky','$fcablesh','$fcableshe','$tcablesh','$tcableshe','$tchsh','$tchshe')";
    mysqli_query($con,$sql);
    $msg=array("r"=>"Saved Sucessfully...");

    echo json_encode($msg);

  } else if($splicetype == "2"){
  $sql="insert into a_fiber_connection(`key`,OWNER_CD,created_on,created_by,type,from_cable_id,to_cable_id,from_buffer_color,to_buffer_color,from_fiber_color,to_fiber_color,to_blade_number,to_port_number,job,stage,host_tag,job_key,owner_key,host_site_key,from_cable_sheath_mark, from_cable_sheath_mark_eom, to_cable_sheath_mark, to_cable_sheath_mark_uom,to_chassis_sheath_mark, to_chassis_sheath_mar_uom) values('$autoid','$ownerCD','$createdon','$createdby','S','$fromcable','$tocable','$frombuffer','$tobuffer','$fromfiber','$tofiber','$Tray','$Slot','$jno','$stage','$site','$jobkey','$ownerky','$hostky','$fcablesh','$fcableshe','$tcablesh','$tcableshe','$tchsh','$tchshe')";

    mysqli_query($con,$sql);
    $msg=array("r"=>"Saved Sucessfully...");

    echo json_encode($msg);

   } else if($splicetype == "3"){
 

  	//for($i=0;$i<12;$i++){
     $colorsql="select * from code_colors";
      $colorquery=mysqli_query($con,$colorsql);
       //for($i=0;$i<12;$i++){
      while($colrow=mysqli_fetch_array($colorquery)){
         
        $autoquery=mysqli_query($con,$autoidquery);
        $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT'];


         $fibercolname=$colrow['color'];
         $sql="insert into a_fiber_connection(`key`,OWNER_CD,created_on,created_by,type,from_cable_id,to_cable_id,from_buffer_color,to_buffer_color,from_fiber_color,to_fiber_color,to_blade_number,to_port_number,job,stage,host_tag,job_key,owner_key,host_site_key,from_cable_sheath_mark, from_cable_sheath_mark_eom, to_cable_sheath_mark, to_cable_sheath_mark_uom,to_chassis_sheath_mark, to_chassis_sheath_mar_uom) values('$autoid','$ownerCD','$createdon','$createdby','S','$fromcable','$tocable','$frombuffer','$tobuffer','$fibercolname','$fibercolname','$Tray','$Slot','$jno','$stage','$site','$jobkey','$ownerky','$hostky','$fcablesh','$fcableshe','$tcablesh','$tcableshe','$tchsh','$tchshe')";
    	   mysqli_query($con,$sql);
	}
    $msg=array("r"=>"Saved Sucessfully...");

    echo json_encode($msg);

  }
 
   
   
  } else {
  	$msg=array("r"=> $splicetype);
  	echo json_encode($msg);
  	//$splicetype
  }

   
?>
