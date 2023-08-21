  <?php
  session_start();
  include "conn.php"; 
  include "allfunctions.php";
  //$ownerCD=$_SESSION['oid'];
  $time=$_POST['time']; 

  $ownerCD=$_SESSION['OWNER_CD'];
  $htag=$_POST['htag'];
  $sitekey=getHostkey($htag);

  
  $mfr=$_POST['manufacturer_key'];
   $mfrname=getmanufacturername($mfr);
  
  $mod=$_POST['model_key'];
  $modlname=getModelnumber($mod);
  $modslot=getModelslot($mod);
  $moddesc=getModdesc($mod);

  $slotb=$_POST['slotb'];
  $slotbladetray=$_POST['slotb'];
  $pos=$_POST['pos'];
  $pic=$_POST['pic'];
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
 $protocal=$_POST['protocal'];
 $protocal = mysqli_real_escape_string($con,$protocal);
 $vidioproto=$_POST['vidioproto'];
 $vidioproto = mysqli_real_escape_string($con,$vidioproto);
 $getway=$_POST['getway'];
 $getway = mysqli_real_escape_string($con,$getway);
 $mulip=$_POST['mulip'];
 $mulip = mysqli_real_escape_string($con,$mulip);
 $vlan=$_POST['vlan'];
 $vlan = mysqli_real_escape_string($con,$vlan);
 //  
 
  $manufactured=$_POST['manufactured'];
  $siteid=$_POST['siteid'];
  $stage=$_SESSION['stage'];
  $tagnumber=$_POST['tag'];
  $ownerkey=$_POST['owner_key'];
  $rack_number=$_POST['rack_number'];
  $getrack_key=getracknumber($rack_number);
  
  //$time='now()';
  $uid=$_SESSION['uid'];
  $type="10";
  $actsts=$_POST['actsts'];
  ///
//     $ip=$_SESSION['ip'];
//     if($_SESSION['subnet']=="")
//      $subnet="0";
//    else
//     $subnet=$_SESSION['subnet'];
//   if($_SESSION['protocal']=="")
//     $protocal="0";
//   else
//     $protocal=$_SESSION['protocal'];
// if($_SESSION['vprotocal']=="")
//     $vprotocal="0";
//   else
//     $vprotocal=$_SESSION['vprotocal'];
//   if($_SESSION['gateway']=="")
//     $gateway="0";
//   else
//     $gateway=$_SESSION['gateway'];
//   if($_SESSION['vlan']=="")
//     $vlan="";
//   else
//     $vlan=$_SESSION['vlan'];
  

  //4  
  $sql3="update chassis 
   set updated_on='$time',record_state='H' where OWNER_CD='$ownerCD' and site='$tagnumber' and type='10' and record_state='L',updated_from='$createdfrm'"; 
  

    // $sql2="select * from Site where tag_number='$tag_number1' and type='1' and record_state='L'";

if(isset($_POST['htag'])){
  // $sql="insert into chassis(OWNER_CD,site ,manufacturer,model,position,created_on,created_by,type,pictures,notes,serial_number,stage,IP_address,subnet_mask,protocol,video_protocol,IP_gateway,multicast_IP,vlan) 
  //  values('$ownerCD','$siteid','$mfr','$mod','$pos','$time','$uid','$type','$pic','$comment','$slno','$stage','$ip','$subnet','$protocal','$vprotocal','$gateway','$vlan')";
      $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'chassis'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 

     $sql="insert into chassis(OWNER_CD,site ,manufacturer,model,position,created_on,created_by,type,pictures,notes,serial_number,stage,IP_address,number,`key`,`owner_key`,`rack`,rack_key,site_key,manufacturer_key,model_key,created_from,device_name,subnet_mask,protocol,video_protocol,IP_gateway,multicast_IP,vlan,manufactured) 
   values('$ownerCD','$tagnumber','$mfrname','$modlname','$pos','$time','$uid','$type','$pic','$comment','$slno','$stage','$ip','$actdevnumber','$autoid','$ownerkey','$rack_number','$rack_number','$sitekey','$mfr','$mod','$createdfrm','$moddesc','$subnet','$protocal','$vidioproto','$getway','$mulip','$vlan','$manufactured')";
    //Blade
    $autoidqueryb="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'slot_blade_tray_panel'";
          $autoqueryb=mysqli_query($con,$autoidqueryb);
           $keyassocb=mysqli_fetch_assoc($autoqueryb);
        $autoidb=$keyassocb['AUTO_INCREMENT']; 
     $sqlblade="insert into slot_blade_tray_panel(OWNER_CD,chassis_number,manufacturer,model,created_on,created_by,site,rack,rack_key,chassis_key,`key`,owner_key,site_key,manufacturer_key,model_key,created_from,slot_or_blade_number,stage) values('$ownerCD','$autoid','$mfrname','$modlname','$time','$uid','$htag','$rack_number','$rack_number','$autoid','$autoidb','$ownerkey','$sitekey','$mfr','$mod','$createdfrm','1','$stage')";
   //*************************
  
  
   //OWNER_CD='$ownerCD' and

   $sql2="select * from chassis where site='$htag' and type='10' and OWNER_CD='$ownerCD' and record_state='L'";
   $sql1="select * from chassis where  site='$htag' and type='10' and `number`='$actdevnumber' and record_state='L'";
   $sqlinst="select * from chassis where site_key='$sitekey' and `rack_key`='$rack_number' and `position`='$pos'";
   $instqr=mysqli_query($con,$sqlinst); //if(mysqli_num_rows($instqr)>0){ sts=2  }
   $instrow=mysqli_fetch_array($instqr); 
   $instid=$instrow['id'];
   //***
      $resch=mysqli_query($con,$sql1);
      $row=mysqli_fetch_array($resch); //if(mysqli_num_rows($resch)>0){ sts=1  }
      $ct=$row['id'];
      $chkey=$row['key'];
      $resstmt=mysqli_query($con,$sql2);

if($actsts=="1"){
  //update
  $chassissql="update chassis 
   set updated_on='$time',record_state='H' where `id`='$ct'"; 
   mysqli_query($con,$chassissql);
   $sql="insert into chassis(OWNER_CD,site ,manufacturer,model,position,created_on,created_by,type,pictures,notes,serial_number,stage,IP_address,number,`key`,`owner_key`,`rack`,rack_key,site_key,manufacturer_key,model_key,created_from,device_name,subnet_mask,protocol,video_protocol,IP_gateway,multicast_IP,vlan,manufactured) 
   values('$ownerCD','$tagnumber','$mfrname','$mod','$pos','$time','$uid','$type','$pic','$comment','$slno','$stage','$ip','$actdevnumber','$chkey','$ownerkey','$rack_number','$rack_number','$sitekey','$mfr','$mod','$createdfrm','$moddesc','$subnet','$protocal','$vidioproto','$getway','$mulip','$vlan','$manufactured')";
   //mysqli_query($con,$sql);
   //***********
        $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'chassis'";
          $autoquery=mysqli_query($con,$autoidquery);
           $keyassoc=mysqli_fetch_assoc($autoquery);
        $autoid=$keyassoc['AUTO_INCREMENT']; 
        //********
        mysqli_query($con,$sql);
        $actdevnumber=$actdevnumber+1;
        $pos=$pos+1;
        $msg= array("d" => "updated...","cnumber" => $autoid ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"0");
        echo json_encode($msg);
} else if($actsts=="2"){
  //insert
        // if($modslot=='1')
        // mysqli_query($con,$sqlblade);

         $ctn=1;
        while($slotb>0){
          $autoqueryb=mysqli_query($con,$autoidqueryb);
           $keyassocb=mysqli_fetch_assoc($autoqueryb);
        $autoidb=$keyassocb['AUTO_INCREMENT']; 

          $sqlblade="insert into slot_blade_tray_panel(OWNER_CD,chassis_number,manufacturer,model,created_on,created_by,site,rack,rack_key,chassis_key,`key`,owner_key,site_key,manufacturer_key,model_key,created_from,slot_or_blade_number,stage) values('$ownerCD','$autoid','$mfrname','$modlname','$time','$uid','$htag','$rack_number','$rack_number','$autoid','$autoidb','$ownerkey','$sitekey','$mfr','$mod','$createdfrm','$ctn','$stage')";
           mysqli_query($con,$sqlblade);
           $ctn=$ctn+1;
          $slotb--;
        }

        mysqli_query($con,$sql);

        if($slotbladetray==1){
           //***********
        $autoidqueryp="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'port'";
          $autoqueryp=mysqli_query($con,$autoidqueryp);
           $keyassocp=mysqli_fetch_assoc($autoqueryp);
        $autoidp=$keyassocp['AUTO_INCREMENT']; 
        //********

           $sqlport="insert into port(`key`, record_state, OWNER_CD, owner_key, site, site_key, rack, rack_key, chassis, chassis_key, slot_key, slot_or_blade_number, port_number, created_on, created_by,created_from) values('$autoidp','L','$ownerCD','$ownerkey','$htag','$sitekey','$getrack_key','$rack_number','$actdevnumber','$autoid','$autoidb','$slotbladetray','1','$time','$uid','$createdfrm')";
           mysqli_query($con,$sqlport);
        }
        //$res=mysqli_query($con,$sql1);
       
        //echo json_encode($msg1);
        $actdevnumber=$actdevnumber+1;
        $pos=$pos+1;
        $msg= array("d" => "Saved...","cnumber" => $autoid ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"0");
        echo json_encode($msg);
}else{

      if(mysqli_num_rows($resch)>0){
        //check
         $msg= array("d" => "duplicate","cnumber" => $ct ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"1");
        echo json_encode($msg);
      } else if(mysqli_num_rows($instqr)>0){
         $msg= array("d" => "duplicate warn","cnumber" => $instid ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"2");
        echo json_encode($msg);
      } else{
        //insert
        $ctn=1;
        while($slotb>0){
          $autoqueryb=mysqli_query($con,$autoidqueryb);
           $keyassocb=mysqli_fetch_assoc($autoqueryb);
        $autoidb=$keyassocb['AUTO_INCREMENT']; 
          $sqlblade="insert into slot_blade_tray_panel(OWNER_CD,chassis_number,manufacturer,model,created_on,created_by,site,rack,rack_key,chassis_key,`key`,owner_key,site_key,manufacturer_key,model_key,created_from,slot_or_blade_number,stage) values('$ownerCD','$autoid','$mfrname','$modlname','$time','$uid','$htag','$rack_number','$rack_number','$autoid','$autoidb','$ownerkey','$sitekey','$mfr','$mod','$createdfrm','$ctn','$stage')";
           mysqli_query($con,$sqlblade);
           $ctn=$ctn+1;
          $slotb--;
        }
        mysqli_query($con,$sql);
        
           if($slotbladetray==1){
            //$mfr','$mod
            $chktemplate="select * from port_template where `manufacturer`='$mfr' and `model`='$mod'";
            $templateqry=mysqli_query($con,$chktemplate);
            if(mysqli_num_rows($templateqry)>0){

           //***********
        $autoidqueryp="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'port'";
          $autoqueryp=mysqli_query($con,$autoidqueryp);
           $keyassocp=mysqli_fetch_assoc($autoqueryp);
        $autoidp=$keyassocp['AUTO_INCREMENT']; 
        //********

           $sqlport="insert into port(`key`, record_state, OWNER_CD, owner_key, site, site_key, rack, rack_key, chassis, chassis_key, slot_key, slot_or_blade_number, port_number, created_on, created_by,created_from) values('$autoidp','L','$ownerCD','$ownerkey','$htag','$sitekey','$getrack_key','$rack_number','$actdevnumber','$autoid','$autoidb','$slotbladetray','1','$time','$uid','$createdfrm')";
           mysqli_query($con,$sqlport);
         }
        }

        $actdevnumber=$actdevnumber+1;
         $pos=$pos+1;
        $msg= array("d" => "Saved224","cnumber" => $autoid ,"actnumber"=>$actdevnumber,"postion"=>$pos,"sts"=>"0");
        echo json_encode($msg);
      }
}

      // else{
      //   if($modslot=='1')
      //   mysqli_query($con,$sqlblade);
      //  mysqli_query($con,$sql);
      //  $res=mysqli_query($con,$sql1);
      //  $row=mysqli_fetch_array($res);
      //  $msg= array("d" => "Saved...","cnumber" => $row['id'] ,"actnumber"=>$actdevnumber,"bsql"=>$sqlblade);
      //  echo json_encode($msg);
      // }


   
   
  }

   
?>
