
<?php
  session_start();
  include "conn.php";	  
  include "allfunctions.php";
  $ownerCD=$_SESSION['OWNER_CD']; 
  $ownerkey=$_SESSION['owner_key'];
  $time=$_POST['time'];   
  $tag=$_POST['tagn'];
  $uid=$_SESSION['uid'];
  $tagkey=getHostkey($tag);
  $cablekey=$_POST['cabid'];
  $samt=$_POST['samt']; 
  
 
  $created_from="field collection";

 //
   $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'Slack'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 
  //

  $sql="insert into Slack (`key`, record_state, owner_key, site_key, cable_key, slack_amount, created_on, created_by, created_from) values('$autoid','L','$ownerkey','$tagkey','$cablekey','$samt','$time','$uid','$created_from')"; 

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
