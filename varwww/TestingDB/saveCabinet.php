
	<?php
	include "conn.php";		  
  $time=$_POST['time']; 
   $gpstime=$time;
  $uid=$_POST['uid'];
  $jno=$_POST['jobnum'];
 
  $otag=$_POST['otag']; 
  $otag = mysqli_real_escape_string($con,$otag);
  $oname=$_POST['oname'];
  $oname = mysqli_real_escape_string($con,$oname);
  $owner1=$_POST['oid'];
  $owner1 = mysqli_real_escape_string($con,$owner1);
  $ownerCD=$_POST['OWNER_CD'];
  $ownerCD = mysqli_real_escape_string($con,$ownerCD);
  $tag_number1=$_POST['tag']; 
  $tag_number1 = mysqli_real_escape_string($con,$tag_number1);
  $site_id1=$_POST['site2'];
  $site_id1 = mysqli_real_escape_string($con,$site_id1);
  $type1=$_POST['type2']; 
  $type1 = mysqli_real_escape_string($con,$type1);
  $SiteName1=$_POST['sitname2'];
  $SiteName1 = mysqli_real_escape_string($con,$SiteName1);

  $manufactured1=$_POST['mfd2'];
  $manufactured1 = mysqli_real_escape_string($con,$manufactured1);
  $manufacturer1=$_POST['mfr2'];
  $manufacturer1 = mysqli_real_escape_string($con,$manufacturer1);
  $Model1=$_POST['mod2'];
  $Model1 = mysqli_real_escape_string($con,$Model1);
  $Pictures1=$_POST['pic2']; ///
  $Pictures1 = mysqli_real_escape_string($con,$Pictures1);
  $loct=$_POST['loct']; /// 
  $loct = mysqli_real_escape_string($con,$loct); 
  $record_count1='4';
  $record_state1=$_POST['rs2'];
  $record_state1 = mysqli_real_escape_string($con,$record_state1);
  $created_by1=$uid;
  $created_on1=$time;
 
  $height1=$_POST['height2'];
  $height1 = mysqli_real_escape_string($con,$height1);
  $depth1=$_POST['depth2'];
  $depth1 = mysqli_real_escape_string($con,$depth1);
  $width1=$_POST['width2'];
  $width1 = mysqli_real_escape_string($con,$width1);
  $CLEAR_ZONE_IND1=$_POST['CLEAR_ZONE_IND2'];
  $CLEAR_ZONE_IND1 = mysqli_real_escape_string($con,$CLEAR_ZONE_IND1);
  $roadway=$_POST['roadway'];
  $roadway = mysqli_real_escape_string($con,$roadway);
  $ctype=$_POST['ctype'];
  $ctype = mysqli_real_escape_string($con,$ctype);
  $createdfrm="field collection";
    
  //**************************
  $laneclosure=$_POST['laneclosure'];
  $laneclosure = mysqli_real_escape_string($con,$laneclosure);
  $dotdis=$_POST['dotdis'];
  $dotdis = mysqli_real_escape_string($con,$dotdis);
  $powr=$_POST['powr'];
  $powr = mysqli_real_escape_string($con,$powr);
  $elecsite=$_POST['elecsite'];
  $elecsite = mysqli_real_escape_string($con,$elecsite);
  $comm=$_POST['comm'];
  $comm = mysqli_real_escape_string($con,$comm);
  $commprovider=$_POST['commprovider'];
  $commprovider = mysqli_real_escape_string($con,$commprovider);
  $sitaddr=$_POST['sitaddr'];
  $sitaddr = mysqli_real_escape_string($con,$sitaddr);
  $udsowner=$_POST['udsowner'];
  $udsowner = mysqli_real_escape_string($con,$udsowner);
  $orientationid=$_POST['orientationid'];
  $orientationid = mysqli_real_escape_string($con,$orientationid);
 
  
  $snumber=$_POST['snumber'];
  
   ///////////////From main menu//////////////////
         if(isset($_POST['plansheet']) && $_POST['plansheet']!="")
            $plansheet=$_POST['plansheet'];
          else
            $plansheet="0";
          if(isset($_POST['psitem']) && $_POST['psitem']!="")
            $psitem=$_POST['psitem'];
          else
            $psitem="0";
        
          if(isset($_POST['stage'])) 
            $stage=$_POST['stage'];
          else
            $stage="0";

      $intersection1=$_POST['intersect2'];
      $intersection1 = mysqli_real_escape_string($con,$intersection1);
      $material1=$_POST['material2'];
      $material1 = mysqli_real_escape_string($con,$material1);
      $mounting1=$_POST['mounting2'];
      $mounting1 = mysqli_real_escape_string($con,$mounting1);
      $filter_count1=$_POST['offilter2'];
      $filter_count1 = mysqli_real_escape_string($con,$filter_count1);
      $filter1=$_POST['fltrsize2'];
      $filter1 = mysqli_real_escape_string($con,$filter1);
      $sun_shield1=$_POST['sunshield2'];
      $sun_shield1 = mysqli_real_escape_string($con,$sun_shield1);
      $installed1=$_POST['installed2'];
      $installed1 = mysqli_real_escape_string($con,$installed1);
      $Description1=$_POST['comment2'];
      $Description1 = mysqli_real_escape_string($con,$Description1);
 
      $ELEC_DEV_ID1=$_POST['etc2'];
      $ELEC_DEV_ID1 = mysqli_real_escape_string($con,$ELEC_DEV_ID1);
      $fosc1=$_POST['fosc2'];
      $fosc1 = mysqli_real_escape_string($con,$fosc1);
      $vault1=$_POST['vault2'];
      $vault1 = mysqli_real_escape_string($con,$vault1);
      $DISTANCE_TO_EOTL1=$_POST['trlane2'];
      $DISTANCE_TO_EOTL1 = mysqli_real_escape_string($con,$DISTANCE_TO_EOTL1);
      $bucket_truck1=$_POST['bucket2'];
      $bucket_truck1 = mysqli_real_escape_string($con,$bucket_truck1);
      $serialno=$_POST['serialno'];
      $serialno = mysqli_real_escape_string($con,$serialno);
      $key=$_POST['key'];
      $ktype=$_POST['ktype'];
      $ktype = mysqli_real_escape_string($con,$ktype);
      $traveldir=$_POST['traveldir'];   
      $traveldir = mysqli_real_escape_string($con,$traveldir);
     
  $ground=$_POST['ground'];
  $msg= array("d" => "Sucessfully saved...");
  $msg1= array("d" => "Sucessfully updated...");
 
  $ownerkey=$_POST['owner_key'];
  $usercounty=$_POST['owner_county'];

   $sql2="select * from Site where tag_number='$tag_number1'  and record_state='L'";
   $resstmt=mysqli_query($con,$sql2);
   $resrow=mysqli_fetch_assoc($resstmt);
   $keyval=$resrow['key'];
   $rowid=$resrow['id'];
   //
    $LONGITUDE1=$resrow['LONGITUDE'];
     $LATITUDE1=$resrow['LATITUDE'];
     $offlongi=$resrow['gps_offset_longitude'];
     $offlattti=$resrow['gps_offset_latitude']; 
     $altitude=$resrow['altitude'];
     $accuracy=$resrow['gps_accuracy'];
     $gpstime=$resrow['gps_time'];

   $jobkey=$_POST['jobkey'];
   $sql="insert into Site (owner,OWNER_CD,tag_number,site_id,type,SiteName,manufactured,
    manufacturer,Model,Pictures,created_by,created_on,height,
    depth,width,CLEAR_ZONE_IND,LONGITUDE,LATITUDE,intersection,material,mounting,filter_count,filter,
    sun_shield,installed,Notes,ELEC_DEV_ID,fosc,vault,DISTANCE_TO_EOTL,bucket_truck,job,serial_number,has_ground_rod,has_key,key_type,direction_of_travel,station_number,plan_sheet,plan_sheet_item,stage,gps_time,roadway,COUNTY,gps_accuracy,altitude,OWNER_KEY,JOB_KEY,location,cabinet_type,lane_closure_required,DOT_district,has_power_disconnect,electric_site_key,has_comms,comms_provider,site_street_address,UDS_owner,orientation,manufacturer_key,model_key,created_from,owner_tag_number,gps_offset_longitude,gps_offset_latitude) values('$owner1','$ownerCD','$tag_number1','$tag_number1','$type1','$SiteName1',
    '$manufactured1','$manufacturer1','$Model1','$Pictures1','$created_by1','$created_on1','$height1','$depth1','$width1','$CLEAR_ZONE_IND1',
    '$LONGITUDE1','$LATITUDE1','$intersection1','$material1','$mounting1','$filter_count1','$filter1',
    '$sun_shield1','$installed1','$Description1','$ELEC_DEV_ID1','$fosc1','$vault1',
    '$DISTANCE_TO_EOTL1','$bucket_truck1','$jno','$serialno','$ground','$key','$ktype','$traveldir','$snumber','$plansheet','$psitem','$stage','$gpstime','$roadway','$usercounty','$accuracy','$altitude','$ownerkey','$jobkey','$loct','$ctype','$laneclosure','$dotdis','$powr','$elecsite','$comm','$commprovider','$sitaddr','$udsowner','$orientationid','$manufacturer1','$Model1','$createdfrm','$otag','$offlongi','$offlattti')";

    $sql1="update Site 
   set updated_on='$created_on1',updated_by='$uid',record_state='H' where tag_number='$tag_number1'  and record_state='L'";

      //For update..
     
      if(mysqli_num_rows($resstmt)>0 && isset($_POST['uid'])){
        mysqli_query($con,$sql1);
        //*************************
           $sql="insert into Site (owner,OWNER_CD,tag_number,site_id,type,SiteName,manufactured,
    manufacturer,Model,Pictures,created_by,created_on,height,
    depth,width,CLEAR_ZONE_IND,LONGITUDE,LATITUDE,intersection,material,mounting,filter_count,filter,
    sun_shield,installed,Notes,ELEC_DEV_ID,fosc,vault,DISTANCE_TO_EOTL,bucket_truck,job,serial_number,has_ground_rod,has_key,key_type,direction_of_travel,station_number,plan_sheet,plan_sheet_item,stage,gps_time,roadway,COUNTY,gps_accuracy,altitude,`key`,OWNER_KEY,JOB_KEY,location,cabinet_type,lane_closure_required,DOT_district,has_power_disconnect,electric_site_key,has_comms,comms_provider,site_street_address,UDS_owner,orientation,manufacturer_key,model_key,created_from,owner_tag_number,gps_offset_longitude,gps_offset_latitude) values('$owner1','$ownerCD','$tag_number1','$tag_number1','$type1','$SiteName1',
    '$manufactured1','$manufacturer1','$Model1','$Pictures1','$created_by1','$created_on1','$height1','$depth1','$width1','$CLEAR_ZONE_IND1',
    '$LONGITUDE1','$LATITUDE1','$intersection1','$material1','$mounting1','$filter_count1','$filter1',
    '$sun_shield1','$installed1','$Description1','$ELEC_DEV_ID1','$fosc1','$vault1',
    '$DISTANCE_TO_EOTL1','$bucket_truck1','$jno','$serialno','$ground','$key','$ktype','$traveldir','$snumber','$plansheet','$psitem','$stage','$gpstime','$roadway','$usercounty','$accuracy','$altitude','$keyval','$ownerkey','$jobkey','$loct','$ctype','$laneclosure','$dotdis','$powr','$elecsite','$comm','$commprovider','$sitaddr','$udsowner','$orientationid','$manufacturer1','$Model1','$createdfrm','$otag','$offlongi','$offlattti')";
    //***************
      $sts=mysqli_query($con,$sql);
      $sqlsiteid="select max(id) sid from Site";
      $resid=mysqli_query($con,$sqlsiteid);
      $geSiteid=mysqli_fetch_assoc($resid);      
      $site_id1=$geSiteid['sid'];
      ////////////////      
        $msg1= array("d" => "Sucessfully updated...","sts" =>"1");
        $msg2= array("d" => "Something went wrong.","sts" =>"0");
        if($sts){
               echo json_encode($msg1);
            } else{
               $sql1="update Site set updated_on=NULL,updated_by=NULL,record_state='L' where tag_number='$tag_number1'  and record_state='H' and `id`='$rowid'";
               mysqli_query($con,$sql1);
               echo json_encode($msg2);  
            }
    }else{
      
        $msg= array("d" => "Something went wrong.","sts" =>"0");
        echo json_encode($msg);
      }

//****paramiter variables */
 //time,uid,jobnum,otag,oname,oid,OWNER_CD,tag,site2,type2,sitname2,mfd2,mfr2,mod2,pic2,loct
   //rs2,height2,depth2,width2,CLEAR_ZONE_IND2,roadway,ctype,laneclosure,dotdis,powr,elecsite,
   //comm,commprovider,sitaddr,udsowner,orientationid,snumber,plansheet,psitem,stage,intersect2
   //material2,mounting2,offilter2,fltrsize2,sunshield2,installed2,comment2,etc2,fosc2,vault2,
   //trlane2,bucket2,serialno,key,ktype,traveldir,ground,owner_key,owner_county,jobkey
	
?>