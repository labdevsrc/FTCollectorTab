
<?php
  session_start();
  include "conn.php";	  
  include "allfunctions.php";
  $ownerCD=$_SESSION['OWNER_CD']; 
  $ownerkey=$_SESSION['owner_key'];
  $time=$_POST['time'];   
  $tag=$_SESSION['tag'];
  $uid=$_SESSION['uid'];
  $tagkey=getHostkey($tag);
  $stype=$_POST['stype'];
  $duct=$_POST['fromduct'];  
  $cablekey=$_POST['cabid']; 
  $rackid=$_POST['rackid'];  
  $chassiskey=$_POST['chassisid']; 
  $sm=$_POST['smid']; 
  $uom=$_POST['uom'];  
  $created_from="field collection";

 //
   $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'sheath_mark'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 
  //

  $sql="insert into sheath_mark (`key`, record_state, owner_key, site_key, cable_key, `type`, chassis_key, sheath_mark, uom, created_on, created_by, created_from) values('$autoid','L','$ownerkey','$tagkey','$cablekey','$stype','$chassiskey','$sm','$uom','$time','$uid','$created_from')"; 

	 if(isset($_SESSION['OWNER_CD']) && isset($_SESSION['tag'])){  
	   $shqry=mysqli_query($con,$sql);
		   if($shqry){
		     $msg= array("sts" => "1");        
	          echo json_encode($msg);
	       } else {
	      	 $msg= array("sts" => "0");        
	          echo json_encode($msg);
	       }

	 } else {
	   $msg= array("sts" => "0");        
       echo json_encode($msg);
	}

	 
?>
