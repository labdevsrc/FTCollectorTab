  <?php
  session_start();
// {"htag":htag,"mfr":mfr,"mod":mod,"pos":pos,"rack":rack};
  include "conn.php"; 
  include "allfunctions.php";
  $time=$_POST['time']; 
  //$htag=$_POST['htag'];
  $htag=$_POST['host_tag_number'];
  $htag = mysqli_real_escape_string($con,$htag);
  $sitekey=getHostkey($htag);
  $mfr=$_POST['manufacturer_key'];
  $mfrname=getmanufacturername($mfr);
  $mfrname = mysqli_real_escape_string($con,$mfrname);
  $mod=$_POST['model_key'];
  $modname=getModelnumber($mod);
  $pos=$_POST['position'];
  $pos = mysqli_real_escape_string($con,$pos);
  $rack=$_POST['rack'];
  $pic=$_POST['pic'];
  //$time=$time;
  $uid=$_POST['uid'];
  $type="20";
  $owner_cd=$_POST['OWNER_CD'];
  $installed=$_POST['installed'];
  $installed = mysqli_real_escape_string($con,$installed);
  $note=$_POST['comment'];
   $note = mysqli_real_escape_string($con,$note);
  $enumber=$_POST['enumber'];
  $enumber = mysqli_real_escape_string($con,$enumber);
  $enumbert =$enumber;
  $rnumber=$_POST['rack_number'];
   $rnumber = mysqli_real_escape_string($con,$rnumber);
  $stage=$_SESSION['stage'];
  $ownerkey=$_SESSION['owner_key'];

  //10     

if(isset($_SESSION['tag']) && isset($_SESSION['uid'])) {
  $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'chassis'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

  $sql="insert into chassis(site,manufacturer,model,position,created_on,created_by,type,rack,pictures,OWNER_CD,installed,notes,number,stage,`key`,`owner_key`,rack_key,site_key,manufacturer_key,model_key,created_from)
   values('$htag','$mfrname','$modname','$pos','$time','$uid','$type','$rnumber','$pic','$owner_cd','$installed','$note','$enumber','$stage','$autoid','$ownerkey','$rnumber','$sitekey','$mfr','$mod','$createdfrm')";
   $sql1="select id from chassis where created_on=(select max(created_on) from chassis)";
   $sql2="select * from chassis where rack_key='$rnumber' and `number`='$enumber' and  site='$htag' and created_by='$uid' and OWNER_CD='$owner_cd' and type='$type' and record_state='L'";

  
   $qr=mysqli_query($con,$sql2);
   $chval=mysqli_fetch_assoc($qr);
    // $sql2="select * from Site where tag_number='$tag_number1' and type='1' and record_state='L'";

  //
   $enumber=$enumber+1;
   $msg= array("sts" => "1","enumber"=>$enumber);
    if(mysqli_num_rows($qr) > 0 ){   
      $rowid=$chval['id'];
      $rkey=$chval['key'];
      $sql3="update chassis set updated_from='$createdfrm',updated_on='$time',updated_by='$uid',record_state='H' where `id`='$rowid'"; 
      $up=mysqli_query($con,$sql3);   
      if($up){
        $sql="insert into chassis(site,manufacturer,model,position,created_on,created_by,type,rack,pictures,OWNER_CD,installed,notes,number,stage,`key`,`owner_key`,rack_key,site_key,manufacturer_key,model_key,created_from)
        values('$htag','$mfrname','$modname','$pos','$time','$uid','$type','$rnumber','$pic','$owner_cd','$installed','$note','$enumbert','$stage','$rkey','$ownerkey','$rnumber','$sitekey','$mfr','$mod','$createdfrm')";
       mysqli_query($con,$sql);  
       echo json_encode($msg);   
      }else{
        $msg= array("sts" => "0","enumber"=>'0');
        echo json_encode($msg);
      }
      
    }else{
      mysqli_query($con,$sql); 
      $msg= array("sts" => "1","enumber"=>$enumber);
        echo json_encode($msg);         
      
    } 
    //
    // mysqli_query($con,$sql);    
    // $msg= array("d" => "Saved...");
    // echo json_encode($msg);
   
  }

   
?>
