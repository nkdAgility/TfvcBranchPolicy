using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfvcBranchPolicy.Common
{
   public class TellMe
    {
       private static TelemetryClient instance;

       private TellMe() { }

       public static TelemetryClient Instance
   {
      get 
      {
         if (instance == null)
         {
             instance = new TelemetryClient();
             instance.InstrumentationKey = "01dec352-56bd-43c7-bc96-6838841252c3";
             // Set session data:
             instance.Context.User.Id = Environment.UserName;
             instance.Context.User.AccountId = Environment.UserDomainName;
             instance.Context.Session.Id = Guid.NewGuid().ToString();
             instance.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
             instance.Context.Component.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
         }
         return instance;
      }
   }


    }
}
