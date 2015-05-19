using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzTest
{
    class Program
    {
        static EventWaitHandle eventWait = new EventWaitHandle(false, EventResetMode.ManualReset);
        class DumbJob : IJob
        {
            public DumbJob()
            {
            }

            public void Execute(JobExecutionContext context)
            {
                Console.WriteLine("{0} DumbJob is executing.", DateTime.Now.TimeOfDay);
                //throw new JobExecutionException("Some problem",
                //    null, true);
                throw new Exception("Some problem");
            }
        }

        class SchedulerListener : ISchedulerListener
        {
            public SchedulerListener()
            {
                Console.WriteLine("Scheduler Listener is alive");
            }

            public virtual void JobScheduled(Trigger trigger)
            {
                // Log when the job will be triggered next
                Console.WriteLine("Job Scheduled: " + trigger.JobName);
                Console.WriteLine("Trigger set at time (UTC): " + trigger.StartTimeUtc);
                Console.WriteLine("This trigger will fire next at (UTC): " + trigger.GetFireTimeAfter(DateTime.UtcNow));
            }

            public virtual void JobsPaused(string jobName, string jobGroup)
            {
                Console.WriteLine("Job was paused" + jobName);
            }

            public virtual void JobsResumed(string jobName, string jobGroup)
            {
                Console.WriteLine("Job was resumed: " + jobName);
            }

            public virtual void JobUnscheduled(string triggerName, string triggerGroup)
            {
                Console.WriteLine("Job was unscheduled. Trigger information:" + triggerName);
            }

            public virtual void SchedulerError(string message, SchedulerException exception)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Scheduler error: " + message, exception);
                Console.ForegroundColor = color;
            }

            public virtual void SchedulerShutdown()
            {
                Console.WriteLine("Scheduler is shutting down.");
            }

            public virtual void TriggerFinalized(Trigger trigger)
            {
                Console.WriteLine("Trigger finalized " + trigger.Name);
                eventWait.Set();
            }

            public virtual void TriggersPaused(string triggerName, string triggerGroup)
            {
                Console.WriteLine("Trigger was paused: " + triggerName);
            }

            public virtual void TriggersResumed(string triggerName, string triggerGroup)
            {
                Console.WriteLine("Trigger was resumed: " + triggerName);
            }
        }

        class JobListener : IJobListener
        {
            public JobListener()
            {
                Console.WriteLine("Job Listener is alive");
            }

            public string Name
            {
                get { return "JobListener"; }
            }

            public virtual void JobExecutionVetoed(JobExecutionContext context)
            {
                Console.WriteLine("Job execution was vetoed: " + context.JobDetail.Name);
            }

            public virtual void JobToBeExecuted(JobExecutionContext context)
            {
                Console.WriteLine("Job is about to be executed: " + context.JobDetail.Name);
            }

            public virtual void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException)
            {
                Console.WriteLine("Job was executed: " + context.JobDetail.Name + ". Run time: " + context.JobRunTime.TotalSeconds + " sec.");
            }
        }

        static void Main(string[] args)
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // construct job info
            JobDetail jobDetail = new JobDetail("myJob", null, typeof(DumbJob));
            // fire every hour
            Trigger trigger = TriggerUtils.MakeSecondlyTrigger("MainTrigger", 3, 5);
            // start on the next even hour
            trigger.StartTimeUtc = DateTime.UtcNow;
            trigger.Name = "myTrigger";

            sched.AddSchedulerListener(new SchedulerListener());
            //sched.AddGlobalJobListener(new JobListener());

            sched.ScheduleJob(jobDetail, trigger);

            eventWait.WaitOne();
            Console.WriteLine("Exiting");
            sched.Shutdown();
        }
    }
}
