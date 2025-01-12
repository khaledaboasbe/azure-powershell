﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Management.Monitor.Models;
using System;
using System.Management.Automation;
using Microsoft.WindowsAzure.Commands.Common.CustomAttributes;

namespace Microsoft.Azure.Commands.Insights.Autoscale
{
    /// <summary>
    /// Create an AutoscaleNotification
    /// </summary>
    [CmdletDeprecation(ReplacementCmdletName = "New-AzAutoscaleNotificationObject")]
    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "AutoscaleNotification"), OutputType(typeof(Management.Monitor.Management.Models.AutoscaleNotification))]
    public class NewAzureRmAutoscaleNotificationCommand : MonitorCmdletBase
    {
        private const string Operation = "Scale";

        /// <summary>
        /// Gets or sets the CustomEmails list of the action. A comma-separated list of e-mail addresses
        /// </summary>
        [Parameter(Position = 0, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The list of comma-separated webhooks")]
        public Management.Monitor.Management.Models.WebhookNotification[] Webhook { get; set; }

        /// <summary>
        /// Gets or sets the CustomEmails list of the action. A comma-separated list of e-mail addresses
        /// </summary>
        [Parameter(Position = 1, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The list of custom e-mails")]
        [ValidateNotNullOrEmpty]
        public string[] CustomEmail { get; set; }

        /// <summary>
        /// Gets or sets the send e-mail to subscription administrator flag
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The send e-mail to subscription administrator flag")]
        public SwitchParameter SendEmailToSubscriptionAdministrator { get; set; }

        /// <summary>
        /// Gets or sets the send e-mail to subscription coadministrators flag
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The send e-mail to subscription coadministrators flag")]
        public SwitchParameter SendEmailToSubscriptionCoAdministrator { get; set; }

        /// <summary>
        /// Executes the Cmdlet. This is a callback function to simplify the exception handling
        /// </summary>
        protected override void ProcessRecordInternal()
        { }

        /// <summary>
        /// Execute the cmdlet
        /// </summary>
        public override void ExecuteCmdlet()
        {
            if (!(this.SendEmailToSubscriptionAdministrator || this.SendEmailToSubscriptionCoAdministrator) &&
                ((this.Webhook == null || this.Webhook.Length < 1) && (this.CustomEmail == null || this.CustomEmail.Length < 1)))
            {
                throw new ArgumentException("At least one Webhook or one CustomeEmail must be present, or the notification must be sent to the admin or co-admin");
            }

            var emailNotification = new Management.Monitor.Management.Models.EmailNotification
            {
                CustomEmails = this.CustomEmail,
                SendToSubscriptionAdministrator = this.SendEmailToSubscriptionAdministrator,
                SendToSubscriptionCoAdministrators = this.SendEmailToSubscriptionCoAdministrator,
            };

            var notification = new Management.Monitor.Management.Models.AutoscaleNotification
            {
                Email = emailNotification,
                Webhooks = this.Webhook
            };

            WriteObject(notification);
        }
    }
}
