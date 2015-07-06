using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CASE.IssueTrackingDll.Controls
{
    /// <summary>
    /// Interaction logic for IssuesPanel.xaml
    /// </summary>
    public partial class IssuesPanel : UserControl
    {
       
        public string exeConfigPath;
        //public event RoutedEventHandler ChangeStatus_ClickEH;
        public event RoutedEventHandler AddCommentButt_ClickEH;
        //public event RoutedEventHandler AddIssueEH;
       // public event RoutedEventHandler DelIssueButt_ClickEH;
        public event RoutedEventHandler OpenImageEH;
        public event RoutedEventHandler Open3DViewEH;
        public event RoutedEventHandler ComponentsShowEH;
        //public event EventHandler ApplyFilterEH;
       // public event RoutedEventHandler OpenLinkEH;
        public event SelectionChangedEventHandler filterCombo_SelectionChangedEH;

        public IssueManager IM;

        public IssuesPanel()
        {
            InitializeComponent();

            
        }
        public void updateStatusFilter(List<Status> items)
        {
            List<String> stats = items.Select(item => item.name).ToList();
            statusfilter.updateList(stats);  
        }
        public void updatePriorityFilter(List<Priority> items)
        {
            List<String> stats = items.Select(item => item.name).ToList();
            priorityfilter.updateList(stats);
        }
        public void updateResolutionFilter(List<Resolution> items)
        {
            List<String> stats = items.Select(item => item.name).ToList();
            resolutionfilter.updateList(stats);
        }
         public void updateTypeFilter()
        {
            List<String> types = IM.jira.TypesCollection.Select(item => item.name).ToList();
            typefilter.updateList(types);
        }
        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            IM.getIssues();
        }
        public string Filters
        {
            get
            {
                string filters = "";
                if (customFilters.IsEnabled)
                {
                    filters = statusfilter.Result + typefilter.Result + priorityfilter.Result + assigneefilter.Result + resolutionfilter.Result;
                    filters = (filters!="") ? filters+Order : "";
                }
                else if (filterIndex>0)
                {
               filters = "+AND+"+IM.jira.FiltersCollection[filterIndex].searchUrl.Replace(IM.client.BaseUrl.ToString()+"/search?jql=", "");
                }
                return filters;
            }

        }
        public string Order
        {
            get
            {
                string ordertype = "";
                 string orderdate = "";
                foreach (RadioButton rb in groupb.Children)
                {
                    if (rb.IsChecked.Value)
                    {
                        if(rb.GroupName=="ordertype")
                            ordertype = rb.Content.ToString();
                        else
                            orderdate = rb.Content.ToString();

                    }

                }
               
                return "+ORDER+BY+"+orderdate+"+"+ordertype;
            }

        }
        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            IM.ChangeStatus_Click();
        }
        private void ChangePriority_Click(object sender, RoutedEventArgs e)
        {
            IM.ChangePriority_Click();
        }


        private void OpenLink(object sender, RoutedEventArgs e)
        {
            //yes has to change anc call settings but i'm lazy **********************************************************
            string casedesigninc = "casedesigninc";
            string content = (string)((Button)sender).Tag;
            System.Diagnostics.Process.Start("https://" + casedesigninc + ".atlassian.net/browse/" + content);
        }

        private void AddCommentButt_Click(object sender, RoutedEventArgs e)
        {
            if (AddCommentButt_ClickEH != null)
            {
                AddCommentButt_ClickEH(sender, e);
            }
        }
        private void AddIssue(object sender, RoutedEventArgs e)
        {
            IM.AddIssue();
        }
        private void DelIssueButt_Click(object sender, RoutedEventArgs e)
        {
            //if (DelIssueButt_ClickEH != null)
            //{
            //    DelIssueButt_ClickEH(sender, e);
            //}
            IM.DelIssueButt_Click();
        }
        private void exportIssues_Click(object sender, RoutedEventArgs e)
        {
            IM.ExportIssues();
        }
        private void OpenImage(object sender, RoutedEventArgs e)
        {
            if (OpenImageEH != null)
            {
                OpenImageEH(sender, e);
            }
        }
        private void Open3DView(object sender, RoutedEventArgs e)
        {
            if (Open3DViewEH != null)
            {
                Open3DViewEH(sender, e);
            }
        }
        private void ComponentsShow(object sender, RoutedEventArgs e)
        {
            if (ComponentsShowEH != null)
            {
                ComponentsShowEH(sender, e);
            }
        }

        public string respText
        {
            get { return resp.Text; }
            set { resp.Text = (string)value; }
        }
        public int issueIndex
        {
            get { return issueList.SelectedIndex; }
            set { issueList.SelectedIndex = value; }
        }
        public System.Collections.IList issueItems
        {
            get { return issueList.SelectedItems; }
        }
        public ItemCollection issueItemC
        {
            get { return issueList.Items; }
        }


        public int filterIndex
        {
            get { return filterCombo.SelectedIndex; }
            set { filterCombo.SelectedIndex = value; }
        }

        private void filterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filterCombo_SelectionChangedEH != null)
            {
                filterCombo_SelectionChangedEH(this, e);
            }
        }

        private void clearFilters_Click(object sender, RoutedEventArgs e)
        {
            statusfilter.Clear();
            typefilter.Clear();
            priorityfilter.Clear();
            assigneefilter.Clear();
            resolutionfilter.Clear();
            if (filterCombo.SelectedIndex > 0 && filterCombo.Items.Count > 0)
                filterCombo.SelectedIndex = 0;
            else
                IM.getIssues();
        }

        //private void Resolve_Click(object sender, RoutedEventArgs e)
        //{
        //    IM.Resolve_Click();
        //}

        private void ChangeAssign_Click(object sender, RoutedEventArgs e)
        {
            IM.ChangeAssign_Click();
        }

        private void ChangeType_Click(object sender, RoutedEventArgs e)
        {
            IM.ChangeType_Click();
        }

        private void meetingDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //string date = null;
            //if (meetingDatePicker.SelectedDate.HasValue)
            //    date = meetingDatePicker.SelectedDate.Value.ToString("yyy-MM-dd");
            //IM.ChangeMeetingDate(date);
        }

        private void ChangeComponents_Click(object sender, RoutedEventArgs e)
        {
            IM.ChangeComponents_Click();
        }

        private void ChangeUsers_Click(object sender, RoutedEventArgs e)
        {
            IM.ChangeUsers_Click();
        }

        private void ChangeDate_Click(object sender, RoutedEventArgs e)
        {
            IM.ChangeDate_Click();
        }

       

       

       
        

       

    }
}
