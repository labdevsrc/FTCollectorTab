  <?php
  session_start();
  include "conn.php";   
  $ownerCD=$_POST['OWNER_CD'];

  $rnumber=$_POST['racknumber'];
  $ori=$_POST['orientation'];
  $xpos=$_POST['xpos'];
  $ypos=$_POST['ypos'];
  $mfr=$_POST['manufacturer'];
  $mod=$_POST['model'];
  $mfrkey=$_POST['manufacturer_key'];
  $modkey=$_POST['model_key'];  
  $height=$_POST['height'];
  $width=$_POST['width'];
  $depth=$_POST['depth'];
  //$time='now()';
  $time=$_POST['time']; 
  $uid=$_POST['uid'];
  $siteid= $_POST['tag'];
  $type=$_POST['type'];
  $created_from="field collection";
    //$tag=$_SESSION['tag'];
  $front_back=$_POST['front_back'];
  //4  
  // $sql3="update chassis set updated_on='$time',updated_by='$uid',record_state='H' where site='$htag' and created_by='$uid' and OWNER_CD='$owner_cd' and type='$type' and record_state='L'";
         ////*******Getting owner key******
  $ownerkeysql="select * from `owner` where OWNER_CD='$ownerCD' and record_state='L'";
  $resoquery=mysqli_query($con,$ownerkeysql);
  $resokey=mysqli_fetch_assoc($resoquery);
  $ownerkey=$resokey['key'];
  ////*******Getting Site key********
  $sitekeysql="select * from Site where OWNER_CD='$ownerCD' and record_state='L' and tag_number='$siteid'";
  $ressitequery=mysqli_query($con,$sitekeysql);
  $ressitekey=mysqli_fetch_assoc($ressitequery);
  $sitekey=$ressitekey['key'];
  //echo("Sitekey : ".$sitekey);
  //****
    $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'rack_rail_shelf'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

  $updatesql="update rack_rail_shelf set updated_on='$time',record_state='H' where site_id='$siteid' and created_by='$uid' and OWNER_CD='$ownerCD' and record_state='L' and `number`='$rnumber' and record_state='L'";

  $sql="insert into rack_rail_shelf (OWNER_CD, site_id,number, manufacturer, model,x_position, y_position, orientation, height, width, depth, created_on, front_back, created_by,type,stage,`key`,OWNER_KEY,SITE_KEY,manufacturer_key,model_key,created_from) values('$ownerCD','$siteid','$rnumber','$mfr','$mod','$xpos','$ypos','$ori','$height','$width','$depth','$time','$front_back','$uid','$type','A','$autoid','$ownerkey','$sitekey','$mfrkey','$modkey','$created_from')"; 

  $selectsql="select * from rack_rail_shelf where site_id='$siteid' and created_by='$uid' and OWNER_CD='$ownerCD' and record_state='L' and `number`='$rnumber'";

 if(isset($_POST['OWNER_CD']) && isset($_POST['tag'])){  
    $sel=mysqli_query($con,$selectsql);
    $row=mysqli_fetch_assoc($sel);     
      $keyval=$row['key'];
    if(mysqli_num_rows($sel) > 0 ){
        $sql="insert into rack_rail_shelf (OWNER_CD, site_id,number, manufacturer, model,x_position, y_position, orientation, height, width, depth, created_on, front_back, created_by,type,stage,`key`,OWNER_KEY,SITE_KEY,manufacturer_key,model_key,created_from) values('$ownerCD','$siteid','$rnumber','$mfr','$mod','$xpos','$ypos','$ori','$height','$width','$depth','$time','$front_back','$uid','$type','A','$keyval','$ownerkey','$sitekey','$mfrkey','$modkey','$created_from')"; 
     
      mysqli_query($con,$updatesql);
      mysqli_query($con,$sql);
      echo "1";   
    } else {
        
        // $sql="insert into rack_rail_shelf (OWNER_CD, site_id,number, manufacturer, model,x_position, y_position, orientation, height, width, depth, created_on, created_by,type,stage,`key`) values('$ownerCD','$siteid','$rnumber','$mfr','$mod','$xpos','$ypos','$ori','$height','$width','$depth','$time','$uid','$type','A','$autoid')"; 
      mysqli_query($con,$sql);
       echo "1";   
    }
 } else {
  echo "0";
}

?>


