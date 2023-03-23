using System;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Web.Infrastructure;
using StackExchange.Redis;
using Blazored.Toast.Services;

namespace Web.Pages.Console
{
    public class UiArea
    {
        // TODO: adding the 'Icon', we could automate the menu as well.
        public UiArea(string key, string title, bool initial = false, bool nested = false)
        {
            Key = key;
            Title = title;
            IsActive = initial;
            IsNested = nested;
        }

        public string Key { get; private set; }
        public string Title { get; private set; }
        public bool IsActive { get; set; }
        public bool IsNested { get; private set; }
    }

    public class IndexModel : ComponentBase
    {
        public string Title { get; set; } = "Dashboard";
        public ContactBatch Contacts { get; set; } = new ContactBatch();
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private IConnectionMultiplexer ConnectionMultiplexer { get; set; }
        [Inject]
        private IConfiguration ConfigurationWorker { get; set; }
        [Inject]
        private IManagementService ManagementService {get; set;}
        public List<UiArea> Areas = new List<UiArea>
        {
            { new UiArea("dashboard", "Dashboard", initial:true) },
        };

        public string expressionMessage { get; set; } = null;
        protected override void OnInitialized()
        {
            
        }

        protected override async Task OnInitializedAsync()
        {
            await GetLatestContacts();

            await ConnectionMultiplexer.GetSubscriber().SubscribeAsync("contacts_events", ((channel, message) => {
                ToastService.ShowSuccess("Incoming contact. Please `Refresh`.");
            }));
        }

        private void EnableArea(string key)
        {
            Areas.ForEach(a =>
            {
                a.IsActive = (a.Key == key);
                if (a.Key == key)
                {
                    this.Title = a.Title;
                }
            });
        }

        public void GetDashboard()
        {
            EnableArea("dashboard");
            StateHasChanged();
        }

        public async Task GetLatestContacts()
        {
            Contacts = await ManagementService.GetLatestMessagesAsync("");
        }
    }
}
