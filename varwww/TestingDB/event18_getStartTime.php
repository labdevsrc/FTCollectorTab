<?php
   include "conn.php";		

  $uid=$_POST['uid'];  //1
  $time=$_POST['time']; 
  $gpstime=$time;

select DISTINCT job_events.employee, job_events.event_time , timesheet.user_hour , timesheet.user_minute from Testing.job_events 
inner join Testing.timesheet on job_events.employee = timesheet.employee 
where CURDATE() = (select date(job_events.event_time)) and job_events.employee != $uid and job_events.event_type = 18