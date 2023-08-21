<?php  
 
  $type=$_POST['type'];
  $uid=$_POST['uid'];
  switch ($type) {

  	case 'manufacturer_list':
  		manufacturer_lists();
    	break;
    case 'job_submittal':
      jobsubmittal_list();
      break;      
  	case 'keytype':
  		keyType();
  		break; 
    case 'unitofmeasure':      
      getUnitOfMeasure();
      break;
    case 'equipment_code':
      getEquipmentCode();
      break;       
    case 'equipment_detail':
      getEquipmentDetail();
      break;           
   case 'Site':
      getSite();
      break;  
  	case 'materialcode':
  		getMaterial();
  		break; 
  	case 'mounting':
  		getMounting();
  		break;
    case 'filter_type':
      getFilterType();
      break;
    case 'roadway':
      getRoadwayList();
      break;
    case 'owner_roadway':
      getOwnerRoadways();
      break;      
    case 'intersection':
      getIntersection();
      break;
    case 'electric':
      getElectricCricuit();
      break;
    case 'direction':
      getDirection();
      break;
    case 'dsize':
      getDuctsize();
      break;
    case 'ducttype':
      getDucttype();
      break;
 
    case 'grouptype':
      getGrouptype();
      break;
    case 'devtype':
      getDevtype();
      break;
    case 'model' :
      getModelProp();
      break;
    case 'racknumber':
      getRacknumber();
      break;
    case 'racktype':
      getRacktype();
      break;
    case 'sheath':
      getSheath();
      break;
    case 'reelid':
      getReelid();
      break;
    case 'sbto':
      getSbto();
      break;
    case 'frcable':
      getFrcable();
      break;
    case 'toencloser':
      getToencloser();
      break;
    case 'cable_type':     
      getCableType();
      break;
    case 'cable_structure':
      getCablestructure();
      break;  
      //////////////////////
    case 'side':
      getSide();
      break;  
    case 'code_color':
      getColorCode();
      break;
    case 'conduits':
      getCONDUITS_GROUP();
      break;
    case 'tracewaretag':
      getTracewaretag();
      break;
     //case 'cables':
      //getCables();
      //break;
     case 'owners':
      getOwners();
      break;
    case 'allcountry':
      getAllcountry();
      break;
    case 'fiberinstalltype':
      getInstalltype();
      break;
    case 'ductinstalltype':
      getDuctInstalltype();
      break;
    case 'ductused':
      getDuctused();
      break;
      /////////////////////      
    case 'intersections':
      get_intersection();
      break;
    case 'dimensions':
      get_dimensions();
      break;  
    case 'fltrsizes':
      get_fltrsize();
      break; 
    case 'splicetype':
      get_splicetype();
      break; 
    case 'laborclass':
      get_laborclass();
      break; 
    case 'travellen':
      get_travellens();
      break; 
    case 'bClassification':
      buildingClassification();
      break;        
    case 'code_chassis_type':
      getChassisType();
      break;

    case 'slotbladetray':
      getSlotbladetrayKey();
      break;
    case 'code_port_type':
      getPortType();
      break;     
    case 'port_table':
      getPortTable();
      break;       
    case 'code_locate_point':
      getlocatepoint();
      break;
    case 'gps_point':
      getMaxGPSPoint();
      break;               
    case 'suspend_trace':
      getSuspendedTrace();
      break;
    case 'exclude_site':
      getExcludeSite();
      break;    
    case 'job_phases_detail':
      getJobPhase();
      break;

	case 'equipment_for_checkout':
		getEquipmentCO();
		break;
    case 'event18_start_time':
      getEvent18StartTime();
      break;
   	  
    case 'occupied_crew_member':
      getOtherCrewOccupiedMember();
      break;
   	  
    case 'cabinet_type':
      getCabinetType();
      break;
    default:
      // code...
      break;
    }

function getCabinetType(){
   include "conn.php";     
   $sql="select `key` CabinetTypeKey, CABINET_TYPE_CD CabinetTypeCode, TYPE_DESC from code_cabinet_type where record_state='L' order by TYPE_DESC asc";
       $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }

      echo json_encode($data);  

}

function getOtherCrewOccupiedMember(){
    include "conn.php";     
  $uid=$_POST['uid'];

$sql="select job_events.employee TeamUserKey, CONCAT(eu.first_name,' ', eu.last_name) FullName, STR_TO_DATE (CONCAT(ts.user_hour ,':', ts.user_minute), '%h:%i') StartTime  from job_events 
inner join (select *  from timesheet where CURDATE() = (select date(timesheet.event_time))) ts on job_events.employee = ts.employee
inner join (select * from end_user where record_state='L') eu on job_events.employee = eu.`key` 
where CURDATE() = (select date(job_events.event_time)) and ts.crew_leader != '$uid' and ts.event_type = 15
group by job_events.employee";
       $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }

      echo json_encode($data);  
} 

function getEvent18StartTime(){
    include "conn.php";    
  $uid=$_POST['uid'];
 
$sql="select job_events.employee TeamUserKey, CONCAT(eu.first_name,' ', eu.last_name) FullName, STR_TO_DATE (CONCAT(ts.user_hour ,':', ts.user_minute), '%h:%i') StartTime  from job_events 
inner join (select *  from timesheet where CURDATE() = (select date(timesheet.event_time))) ts on job_events.employee = ts.employee
inner join (select * from end_user where record_state='L') eu on job_events.employee = eu.`key` 
where CURDATE() = (select date(job_events.event_time)) and job_events.employee != '$uid' and job_events.event_type = 18
group by job_events.employee";
       $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }

      echo json_encode($data);  
}  
	
 function getEquipmentCO(){
    include "conn.php";     
      $sql="SELECT code_equipment_type.key TypeKey, equipment.type EqKey, code_equipment_type.description EquipmentTypeDesc, equipment.description EqDesc FROM Florida.code_equipment_type 
left outer join equipment on  equipment.type = code_equipment_type.key";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }

      echo json_encode($data);  
}    
	
	
	
 function getJobPhase(){
    include "conn.php";     
      $sql="select `key` JobPhaseKey, job JobNumber, job_phase_number PhaseNumber, text Description from job_phase where record_state='L'";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }

      echo json_encode($data);  
}    


    
 function getExcludeSite(){
    include "conn.php";     
      $sql="select  * from exclude_site where record_state='L'";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }

      echo json_encode($data);  
}    


 function getSuspendedTrace(){
    include "conn.php";     
      $sql="select distinct owner_key OwnerKey , job JobNumber, from_site FromDuctSite, from_site_key FromDuctSiteKey, from_site_duct FromDuctKey from a_fiber_segment where to_site IS NULL and from_site IS NOT NULL";
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }

      echo json_encode($data);  
}    

 function getlocatepoint(){
    include "conn.php";     
      $sql="select `id` as IdLocatePoint, description from code_locate_point_type";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }
      echo json_encode($data);  
}

 function getPortTable(){
    include "conn.php";     
      $sql="select `key` as PortKey, site , site_key , owner_key, xmt_rcv,
        rack_key, chassis_key, slot_or_blade_number , port_number 
       from port where record_state='L'";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }
      echo json_encode($data);    
}

 function getPortType(){
    include "conn.php";     
      $sql="select `key` as CodeKey, text as TextType, xmt_rcv TXRX from code_port_type where record_state='L'";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }
      echo json_encode($data);    
}
 function getMaxGPSPoint(){
      include "conn.php";   
    $autoidquery="SELECT `AUTO_INCREMENT` FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '$database' AND   TABLE_NAME   = 'gps_point'";
      $autoquery=mysqli_query($con,$autoidquery);
      $keyassoc=mysqli_fetch_assoc($autoquery);
      $autoid=$keyassoc['AUTO_INCREMENT']; 

      $data = array();
      $data[] = array("MaxId"=>$autoid);
      echo json_encode($data);      

 }
 function getSlotbladetrayKey(){
    include "conn.php";     
      //$sql="select `key` SlotBladeKey, ownerkey OwnerKey from slot_blade_tray_panel where record_state='L'";
      $sql="select `key`,owner_key,rack_key, chassis_key, OWNER_CD, site, slot_or_blade_number, front_back,
        orientation, manufacturer, manufacturer_key, model, model_key, port_count
       from slot_blade_tray_panel where record_state='L'";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }
      echo json_encode($data);    
}

 function manufacturer_lists(){
    include "conn.php";     
    $sql="select manufacturer.name mfname,manufacturer.id mfid,manufacturer.`key` mfkey from manufacturer where manufacturer.record_state='L' order by manufacturer.name";
     $res= mysqli_query($con,$sql); 
     if(mysqli_num_rows($res)>0)  {
        while($row=mysqli_fetch_array($res)){
          $mfnr=$row['mfname'];
          $mfnr=htmlspecialchars($mfnr, ENT_QUOTES | ENT_COMPAT | ENT_IGNORE, 'UTF-8');
     $mfnrv = <<<EOD
        $mfnr
EOD;

     $data[] = array("ManufName"=>$mfnrv,"ManufId"=>$row['mfid'],"ManufKey"=>$row['mfkey']);
   }
  }else{
     $data[]= array("ManuName"=>"none");
   }   
  echo json_encode($data);
}

 function jobsubmittal_list(){
    include "conn.php";     
      $sql="select job_submittal.`key` JobSubKey, job_submittal.job as JobSubJobId, job_submittal.manufacturer as JobSubManuf,
      job_submittal.model as JobSubModel , job_submittal.pay_item_description as JobSubDesc , job_submittal.pay_item as JobSubPayItem 
      from job_submittal
      where job_submittal.record_state='L'";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }


      echo json_encode($data);    
}

 function keyType(){
    include "conn.php";     
      $sql="select `key` KeyTypeKey, KEY_TYPE_DESC KeyTypeDesc from code_key_type where record_state='L'";
      $res= mysqli_query($con,$sql);  
      $data = array();
      while($row = mysqli_fetch_assoc($res)){
        $data[] = $row;
      }


      echo json_encode($data);    
}



function getEquipmentCode(){
    include "conn.php";     
    $sql="select `key` EquipCodeKey, description EquipCodeDesc from code_equipment_type where record_state='L'";
    $res= mysqli_query($con,$sql);  
    $data = array();
    while($row = mysqli_fetch_assoc($res)){
      $data[] = $row;
    }
    echo json_encode($data);  
}

function getUnitOfMeasure(){
    include "conn.php";     
    $sql="select `key` UOMKey, unit UOMUnit, abreviation UOM_Abv from code_unit_of_measure where record_state='L'";
    $res= mysqli_query($con,$sql);  
    $data = array();
    while($row = mysqli_fetch_assoc($res)){
      $data[] = $row;
    }
    echo json_encode($data);  
}

function getEquipmentDetail(){

    include "conn.php";     
    $sql="select `type` EquipmentType, number EquipmentNumber, description EquipmentDesc from equipment";
    $res= mysqli_query($con,$sql);  
    $data = array();
    while($row = mysqli_fetch_assoc($res)){
      $data[] = $row;
    }
    echo json_encode($data);  
}

 function getMaterial(){
    include "conn.php";     
    $sql="select id MaterialId,description CodeDescription,`key` MaterialKey from code_material where record_state='L'";
    $res= mysqli_query($con,$sql);  
    $data = array();
    while($row = mysqli_fetch_assoc($res)){
      $data[] = $row;
    }


    echo json_encode($data);    
}

 function getMounting(){
  include "conn.php"; 
  $sql="select code_mounting.type MountingType ,code_mounting.key MountingKey from code_mounting where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

 function getFilterType(){
  include "conn.php"; 
  $sql="select FILTER_TYPE_DESC as FilterTypeDesc,`key` as FilterTypeKey from code_filter_type where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

 function getRoadwayList(){
  include "conn.php"; 
  $sql = "select roadway.key RoadwayKey,  roadway.name RoadwayName, owner_roadways.owner RoadOwnerKey from roadway 
inner join owner_roadways on owner_roadways.roadway = roadway.key
where roadway.record_state='L'";

  //$sql="select `key` RoadwayKey,name RoadwayName  from roadway where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

  // obsolete
 /*function getOwnerRoadways(){
  include "conn.php"; 
  $sql="select id OR_Id, roadway OR_Roadway, owner OR_Owner from owner_roadways where record_state='L' ";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}*/

 function getIntersection(){
  include "conn.php"; 
  //$sql="select id IntersectionId ,name IntersectionName,`key` IntersectionKey , major_roadway, minor_roadway, OWNER_CD from intersection where `key`!='' and record_state='L'";
  $sql = "select distinct intersection.id IntersectionId , intersection.name IntersectionName,intersection.`key` IntersectionKey , major_roadway, minor_roadway, OWNER_CD, 
roadway.name as MajorRoadwayName , rwy2.name as MinorRoadwayName
from intersection 
inner join roadway on roadway.key = intersection.major_roadway 
inner join roadway rwy2 on rwy2.key = intersection.minor_roadway 
where intersection.`key`!='' and intersection.record_state='L'";

  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}


 function getElectricCricuit(){
  include "conn.php"; 
  $sql="select ELEC_DEV_ID,ELEC_CIRCUIT_NAME from ELECTRIC_DEV_CIRCUIT where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}


 function getDirection(){
  include "conn.php"; 
  $sql="select description as DirDesc, `key` as DirKey from code_compass_direction where record_state='L' order by `key` desc";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}


 function getDuctsize(){
  include "conn.php"; 
  $sql="select `key` as DuctKey, DUCTS_SIZE as Ductsize from code_duct_size where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

 function getDuctUsage(){
  include "conn.php"; 
  $sql="select `key` as DucUsageKey, usage as DuctUsage from code_duct_usage  where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

 function getDucttype(){
  include "conn.php"; 
  $sql="select `key` as DucTypeKey, DUCT_TYPE_DESC as DucTypeDesc from code_duct_type where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

 function getGrouptype(){
  include "conn.php"; 
  $sql="select group_type as Grouptype  from ft_list_groups";
  $res= mysqli_query($con,$sql);  
  if(mysqli_num_rows($res)>0) {
    while($row=mysqli_fetch_array($res)){     
      $data[] = array("Grouptype"=>$row[0],"id1"=>$row[0]);
    }
    }else{
      $data[]= array("Grouptype"=>"none");
    }   
  echo json_encode($data);
}



 function getDevtype(){
  include "conn.php"; 
  $sql="select distinct description DevTypeDesc ,code_model_type.`key` DevTypeKey from model,code_model_type where code_model_type.number in(select type from model)";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}


function getModelProp(){
  include "conn.php"; 

  //$sql="select `key`, manufacturer_key , MODEL_DESC,  number , type , model_type_code_key , height, width, depth, pic_url from model where record_state='L'";
  $sql="select model.key, manufacturer_key , MODEL_DESC,  model.number , code_model_type.description ModelType , model_type_code_key , height, width, depth, pic_url from model 
left outer join code_model_type on code_model_type.key = model.type 
  where model.record_state='L'";  
  $res= mysqli_query($con,$sql);  
  $data = array();

  if(mysqli_num_rows($res)>0)  {
        while($row=mysqli_fetch_array($res)){
          $model_desc=$row['MODEL_DESC'];
          $model_desc=htmlspecialchars($model_desc, ENT_QUOTES | ENT_COMPAT | ENT_IGNORE, 'UTF-8');
     $row['MODEL_DESC'] = <<<EOD
        $model_desc
EOD;
         $data[] = array("ModelDescription"=>$model_desc,"ModelKey"=>$row["key"], "ManufKey"=>$row["manufacturer_key"], "ModelNumber"=>$row["number"],"ModelType"=>$row["ModelType"],
       "ModelCode1"=>$row["type"], "ModelCode2"=>$row["model_type_code_key"],"ModelKey"=>$row["key"], "height"=>$row["height"], "width"=>$row["width"], "depth"=>$row["depth"],
     "PictUrl"=>$row["pic_url"],);
   }
  }
  echo json_encode($data);  
  }

 function getRacknumber(){
  include "conn.php"; 
  //$sql="select `key` as RackNumKey ,number as Racknumber, site_id as SiteId from rack_rail_shelf where record_state='L'";
$sql="SELECT rack_rail_shelf.`key` as RackNumKey, OWNER_CD, number Racknumber, site_id as SiteId, code_rack_type.RACK_MTL_DESC as RackType FROM backup_of_myfibertrak.rack_rail_shelf
left outer  join code_rack_type on rack_rail_shelf.type = code_rack_type.key 
 where rack_rail_shelf.record_state='L'";

  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

 function getRacktype(){
  include "conn.php"; 
  $sql="select `key` as RackTypeKey, RACK_MTL_DESC as RackMaterialDesc from code_rack_type where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}


 function getSheath(){
  include "conn.php"; 
  $sql="select `key` as SheathKey, sheath_type as SheathType from code_fiber_sheath_type where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

 function getReelid(){
  include "conn.php"; 
  $sql="select `key` as ReelKey, number as ReelNumber, job as JobNum from a_fiber_reel where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}


function getSbto(){
  include "conn.php"; 
  $sql="select distinct orientation_of_slots_or_trays from model where orientation_of_slots_or_trays<>'N' and orientation_of_slots_or_trays<>''";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
      $orientation="";
      if($row['orientation_of_slots_or_trays']=="H"){
        $orientation="Horizontal";
      } else if($row['orientation_of_slots_or_trays']=="V"){
        $orientation="Vertical";
      }
      $data[] = array("OrientationDetail"=>$orientation,"Orientation"=>$row['orientation_of_slots_or_trays']);
    }
    echo json_encode($data);  
  }
  


/*function getFrcable(){
  include "conn.php"; 
  $sql="select cable_id as CableIdDesc, model_key as Model, cable_type as CableType, manufacturer_key as Manufacturer ,`key` as AFRKey, job JobNumber, job_key JobKey, owner_key OwnerKey,
  SM_COUNT_CD SMCount, MM_COUNT_CD MMCount from a_fiber_cable where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}*/

function getFrcable(){
  include "conn.php"; 
  $sql="select cable_id as CableIdDesc, model_key as Model, cable_type as CableType, code_cable_type.description as CableTypeDesc, manufacturer_key as Manufacturer ,a_fiber_cable.`key` as AFRKey, 
job JobNumber, job_key JobKey, a_fiber_cable.owner_key OwnerKey, buffer_count, label, MM_diameter,
  SM_COUNT_CD SMCount, MM_COUNT_CD MMCount, code_fiber_sheath_type.sheath_type SheathType from a_fiber_cable 
  left outer join code_fiber_sheath_type on code_fiber_sheath_type.`key` = a_fiber_cable.FIBER_SHEATH_TYPE_CD
  left outer join code_cable_type on code_cable_type.`key` = a_fiber_cable.cable_type
  where a_fiber_cable.record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}


 function getToencloser(){
  include "conn.php";
  //$ownerCD=$_POST['OWNER_CD'];  
  //$tag=$_POST['tag']; 
  $sql="select distinct number,model,`key`, model_key, OWNER_CD, site, rack from chassis where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  if(mysqli_num_rows($res)>0) {
    while($row=mysqli_fetch_array($res)){
         #$mod=$row['model_key'];
         #$modsql="select * from model where `key`='$mod'";  
         #$modquery=mysqli_query($con,$modsql);  
         #$modres=mysqli_fetch_assoc($modquery); 
         #$mnumber=$modres['number'];      
      $data[] = array("Model"=>$row['model'],"ChassisKey"=>$row['key'], "OWNER_CD"=>$row['OWNER_CD'],
        "ModelKey"=>$row['model_key'],"ChassisNum"=>$row['number'],"rack_number"=>$row['rack'],
        "TagNumber"=>$row['site']);
    }
    }else{
      $data[]= array("ChassisNum"=>"none");
    }   
  echo json_encode($data); 
}


 function getChassisType(){
  include "conn.php";

  $sql="select `key` CTKey, description CTDesc from code_chassis_type where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data); 
}

function get_dimensions(){
  include "conn.php";
  // $model=$_POST['model']; 
  $sql="select height,depth,width, `key` as  DimKey, id as DimId from model";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data); 
}

 function get_fltrsize(){  
  
  include "conn.php";
  //$roadway=$_POST['roadway']; 
  $sql="select * from code_filter_size where record_state='L'";
  $res= mysqli_query($con,$sql);  
  if(mysqli_num_rows($res)>0) {
    while($row=mysqli_fetch_array($res)){
      
    // $data[] = array("name1"=>$row['id'],"id1"=>$row['FILTER_SIZE_DESC']);
     $tmp=$row["FILTER_SIZE_DESC"];
    // $tmp=json_encode($tmp);
     $tmp=htmlspecialchars($tmp, ENT_QUOTES, 'UTF-8');
     $tmps = <<<EOD
        $tmp
EOD;
     //12" x 18" x 1"    $row['FILTER_SIZE_DESC']
    $data[] = array("data"=> $tmp ,"FtSizeKey"=>$row['key']);
    }
    }else{
      $data[]= array("data"=>"none");
    }   
  echo json_encode($data);
}

function get_splicetype(){  
  include "conn.php";
  $sql="select text SpliceText,`key` SpliceTypeId from code_splice_type where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}


function get_laborclass(){  
  
  include "conn.php";
  $sql="select text,`id` as LaborClassId,abbreviation from code_labor_classification";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}


 function getCablestructure(){
  include "conn.php"; 
  $sql="select `key` as CableKey, buffer_type as BufferType from a_fiber_reel";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}

////////////////////////////////////////////////
function getCableType(){
  include "conn.php"; 
  $sql="select `key` as CodeCableKey, description as CodeCableDesc, ITSFM from code_cable_type";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);  
}

////////////////////////////////////////////////
function getColorCode(){
  include "conn.php"; 
  $sql="select `key` as ColorKey, color as ColorName, color_hex as ColorHex from code_colors";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);  
}

function getCONDUITS_GROUP(){
  include "conn.php"; 
// BEFORE
   /*$sql="SELECT CONDUITS_GROUP.`key` as ConduitKey, code_compass_direction.ITSFM as Direction, direction_count as DirCnt, host_tag_number as HosTagNumber, USAGE_CD as DuctUsage, DUCT_SIZE_CD as DuctSize, color as DuctColor , code_site_type.major_type HostType, code_site_type.`key` HostTypeKey
   , owner_key OwnerKey, in_use_percent InUsePercent
FROM backup_of_myfibertrak.CONDUITS_GROUP, code_site_type,code_compass_direction 
where CONDUITS_GROUP.direction = code_compass_direction.key and CONDUITS_GROUP.host_type = code_site_type.key and  CONDUITS_GROUP.record_state = 'L'";*/
// AFTER
$sql="SELECT DISTINCT CONDUITS_GROUP.`key` as ConduitKey, code_compass_direction.ITSFM as Direction, direction_count as DirCnt, host_tag_number as HosTagNumber, USAGE_CD as DuctUsage, DUCT_SIZE_CD as DuctSize, color as DuctColor , code_site_type.major_type HostType, code_site_type.`key` HostTypeKey
   , CONDUITS_GROUP.owner_key OwnerKey, in_use_percent InUsePercent, Site.`key` HostSiteKey , CONDUITS_GROUP.created_by CreatedBy  
FROM CONDUITS_GROUP
INNER JOIN Site ON  CONDUITS_GROUP.host_tag_number = Site.tag_number
INNER JOIN code_site_type ON  CONDUITS_GROUP.host_type = code_site_type.`key`
INNER JOIN code_compass_direction ON CONDUITS_GROUP.direction = code_compass_direction.key
where CONDUITS_GROUP.record_state = 'L' ";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}


function getSite(){
    include "conn.php";
    $sql = "select Site.key SiteKey, Site.job_key JobKey, SiteName, Site.owner_key OwnerKey, OWNER_CD , tag_number TagNumber, site_id SiteId, type SiteTypeKey, cabinet_type CabinetTypeKey, building_type BuildingTypeKey, LONGITUDE, LATITUDE, Site.created_by CreatedBy, Site.job JobNumber, cs.site_type SiteTypeDesc from Site inner join (select * from code_site_type where code_site_type.record_state='L') as cs on Site.type = cs.key where Site.record_state = 'L'";
    /*$sql1="select Site.key as SiteKey, Site.job_key as JobKey, Site.owner_key as OwnerKey, OWNER_CD , tag_number as TagNumber, 
site_id as SiteId, type as SiteTypeKey, cabinet_type as CabinetTypeKey, building_type as BuildingTypeKey, LONGITUDE, 
LATITUDE, Site.created_by as CreatedBy, Site.job as JobNumber, code_site_type.major_type as SiteTypeDesc, code_site_type.`key` as SiteTypeKey 
from Site  
inner join code_site_type on Site.`key` = code_site_type.`type`
     where Site.record_state = 'L'";*/
  $res= mysqli_query($con,$sql);       
       $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}

//
function getTracewaretag(){
  include "conn.php"; 
  session_start();
  $ownerCD=$_POST['OWNER_CD'];
  $ownerkey=$_POST['ownerkey'];
  $sql="select * from Site where record_state='L'  and `tag_number` in(select host_tag_number from CONDUITS_GROUP where record_state='L') group by tag_number order by tag_number asc";
  $res= mysqli_query($con,$sql);  
  if(mysqli_num_rows($res)>0) {
    while($row=mysqli_fetch_array($res)){     
      $data[] = array("SiteTagNumber"=>$row['tag_number'],"SiteKey"=>$row['key'],"SiteOwnerKey"=>$row['owner']);
    }
    }else{
      $data[]= array("name1"=>"none");
    }   
  echo json_encode($data);
}



function getCables(){

} 
//

function getOwners(){

  include "conn.php";
  $sql="SELECT end_user_key EndUserKey , alt_owner_key AltOwnerKey , oon.name OwnerName FROM Florida.end_user_owner_xref 
inner join (select * from owner where owner.record_state = 'L') oon on oon.key = alt_owner_key
where end_user_owner_xref.record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}
//
function getAllcountry(){
  include "conn.php";   
  $sql="select id CountyId ,county CountyName from code_county";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}
//

function getInstalltype(){
  include "conn.php";   
  $sql="select `key` FbrInstallKey, description FbrInstallDesc from code_fiber_install_type where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}

function getDuctInstalltype(){
  include "conn.php";   
  $sql="select `key` DuctInstallKey, DUCT_INSTALLATION_DESC DuctInstallDesc from code_duct_installation where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}
//
function getDuctused(){
  include "conn.php";   
  $sql="select `key` DuctKey, code_duct_usage.usage DuctUsage from code_duct_usage where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}


///////////////////////////////////////////////


function get_travellens(){  
  
  include "conn.php";
  //$roadway=$_POST['roadway']; 
  $sql="select `key` as CompasKey, description as CompassDirDesc,ITSFM from code_compass_direction where record_state='L'";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }  
  echo json_encode($data);
}


 function buildingClassification(){
  include "conn.php"; 
  $sql="select `key` as BuildingTypeKey , TYPE_DESC  from code_building_type where record_state='L' order by TYPE_DESC asc";
  $res= mysqli_query($con,$sql);  
  $data = array();
  while($row = mysqli_fetch_assoc($res)){
    $data[] = $row;
  }
  echo json_encode($data);    
}



?>
