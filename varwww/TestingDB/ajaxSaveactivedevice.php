  <?php
  session_start();
  include "conn.php"; 
  include "allfunctions.php";
  //$ownerCD=$_SESSION['oid'];
  $time=$_POST['time']; 

 $ownerCD=$_POST['OWNER_CD'];
  $siteid=$_POST['tag'];
  $stage=$_POST['stage'];
  $tagnumber=$_POST['tag'];
  $ownerkey=$_POST['owner_key'];
  $uid=$_POST['uid']; 

  $htag=$_POST['host_tag_number'];
  $sitekey=getHostkey($htag);

  
  $mfr=$_POST['manufacturer_key'];
   $mfrname=getmanufacturername($mfr);
  
  $mod=$_POST['model_key'];
  $modlname=getModelnumber($mod);
  $modslot=getModelslot($mod);
  $moddesc=getModdesc($mod);

  $slotb=$_POST['slotblade'];
  $slotbladetray=$_POST['slotblade'];
  $pos=$_POST['position'];
  $pic="";//$_POST['pic'];
  $slno=$_POST['slno'];
  $slno = mysqli_real_escape_string($con,$slno);
  $comment=$_POST['comment'];
  $comment = mysqli_real_escape_string($con,$comment);
  $actdevnumber=$_POST['actdevnumber']; 
  $actdevnumber = mysqli_real_escape_string($con,$actdevnumber); 
   $ip=$_POST['ipaddr'];
  $ip = mysqli_real_escape_string($con,$ip);
  //
  $subnet=$_POST['subnet'];
  $subnet = mysqli_real_escape_string($con,$subnet);
 $protocal=$_POST['protocol'];
 $protocal = mysqli_real_escape_string($con,$protocal);
 $vidioproto=$_POST['vidioproto'];
 $vidioproto = mysqli_real_escape_string($con,$vidioproto);
 $getway=$_POST['getway'];
 $getway = mysqli_real_escape_string($con,$getway);
 $mulip=$_POST['multicastip'];
 $mulip = mysqli_real_escape_string($con,$mulip);
 $vlan=$_POST['vlan'];
 $vlan = mysqli_real_escape_string($con,$vlan);
 //  
 
  $manufactured=$_POST['manufactured_date'];

  $rack_number=$_POST['rack_number'];
  $getrack_key=getrackkey($rack_number, $siteid);
  //echo("sitekey ".$sitekey.", getrack_key ".$getrack_key);
  //$time='now()';

  $type="10";
  $createdfrm="collector apps";
  $actsts=$_POST['actsts'];


  //4  
  $sql3="update chassis 
   set updated_on='$time',record_state='H' where OWNER_CD='$ownerCD' and site='$tagnumber' and type='10' and record_state='L',updated_from='$createdfrm'"; 
  

    // $sql2="select * from Site where tag_number='$tag_number1' and type='1' and record_state='L'";

if(isset($_POST['host_tag_number'])){
  // $sql="insert into chassis(OWNER_CD,site ,manufacturer,model,position,created_on,created_by,type,pictures,notes,serial_number,stage,IP_address,subnet_mask,protocol,video_protocol,IP_gateway,multicast_IP,vlan) 
  //  values('$ownerCD','$siteid','$mfr','$mod','$pos','$time','$uid','$type','$pic','$comment','$slno','$stage','$ip','$subnet','$protocal','$vprotocal','$gateway','$vlan')";
      $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'chassis'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

     $sql="insert into chassis(OWNER_CD,site ,manufacturer,model,position,created_on,created_by,type,pictures,notes,serial_number,stage,IP_address,number,`key`,`owner_key`,`rack`,rack_key,site_key,manufacturer_key,model_key,created_from,device_name,subnet_mask,protocol,video_protocol,IP_gateway,multicast_IP,vlan,manufactured) 
   values('$ownerCD','$tagnumber','$mfrname','$modlname','$pos','$time','$uid','$type','$pic','$comment','$slno','$stage','$ip','$actdevnumber','$autoid','$ownerkey','$rack_number','$getrack_key','$sitekey','$mfr','$mod','$createdfrm','$moddesc','$subnet','$protocal','$vidioproto','$getway','$mulip','$vlan','$manufactured')";

   //*************************
  
  
   //OWNER_CD='$ownerCD' and



   $sqlinst="select * from chassis where site_key='$sitekey' and `rack_key`='$getrack_key' and `position`='$pos' and record_state='L'";
   $instqr=mysqli_query($con,$sqlinst);
   $instrow=mysqli_fetch_array($instqr);
   $instid=$instrow['id'];
   $ct=$instrow['id'];
   $chkey=$instrow['key'];
   //***


   /*$sql1="select * from chassis where  site='$htag' and type='10' and `number`='$actdevnumber' and record_state='L'";
   $resch=mysqli_query($con,$sql1);
   $row=mysqli_fetch_array($resch);
   $ct=$row['id'];
   $chkey=$row['key'];*/


if($actsts=="1"){
  //update
  $chassissql="update chassis 
   set updated_on='$time',record_state='H' where `id`='$ct'"; 
   mysqli_query($con,$chassissql);
   $sql="insert into chassis(OWNER_CD,site ,manufacturer,model,position,created_on,created_by,type,pictures,notes,serial_number,stage,IP_address,number,`key`,`owner_key`,`rack`,rack_key,site_key,manufacturer_key,model_key,created_from,device_name,subnet_mask,protocol,video_protocol,IP_gateway,multicast_IP,vlan,manufactured) 
   values('$ownerCD','$tagnumber','$mfrname','$modlname','$pos','$time','$uid','$type','$pic','$comment','$slno','$stage','$ip','$actdevnumber','$chkey','$ownerkey','$rack_number','$getrack_key','$sitekey','$mfr','$mod','$createdfrm','$moddesc','$subnet','$protocal','$vidioproto','$getway','$mulip','$vlan','$manufactured')";
   //mysqli_query($con,$sql);
   //***********
        $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'chassis'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 
        //********
        mysqli_query($con,$sql);
        
        //$actdevnumber=$actdevnumber+1; /// only for web / js client
        //$pos=$pos+1; /// only for web / js client
        
        $msg= array("d" => "updated...","cnumber" => $autoid ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"3");
        echo json_encode($msg);
  } 
  else if($actsts=="0"){
   //if(mysqli_num_rows($resch)>0){
   //      $msg= array("d" => "duplicate","cnumber" => $ct ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"1");
   //     echo json_encode($msg);    
   //}
   //else 
   if(mysqli_num_rows($instqr)>0){
      //$actsts="2";
        $msg= array("d" => "duplicate warn","cnumber" => $instid ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"0");
        echo json_encode($msg);
   }
   else
   {
      $ctn=1;
      mysqli_query($con,$sql); // run  $sql="insert into chassis(OWNER
      $msg= array("d" => "Saved.. $ct".$ct,"cnumber" => $autoid ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"1");
      echo json_encode($msg);
    } // else
   
  }//else if($actsts=="0"){

   } //if(isset($_POST['host_tag_number'])){
?>
