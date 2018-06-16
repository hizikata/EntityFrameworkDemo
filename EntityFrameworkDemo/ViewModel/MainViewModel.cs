using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using System.Linq;
using EntityFrameworkDemo.DAL;
using System.Windows;


namespace EntityFrameworkDemo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        MainService MainSer = new MainService();
        private IEnumerable<Employee> AllEmployees;
        #endregion
        #region Properties
        private ObservableCollection<Employee> _displayInfo;
        public ObservableCollection<Employee> DisplayInfo
        {
            get { return _displayInfo; }
            set { _displayInfo = value; this.RaisePropertyChanged(() => DisplayInfo); }
        }
        private List<string> _name;

        public List<string> Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }


        #endregion
        #region Constructors
        public MainViewModel()
        {
            this.GetAllEmployees();
        }
        #endregion
        #region Commands
        public RelayCommand<object> Search => new RelayCommand<object>((obj) => ExecuteSearch(obj));      
        public RelayCommand Export => new RelayCommand(() => ExecuteExport());
        public RelayCommand<object> Update => new RelayCommand<object>((obj) => ExecuteUpdate(obj));
        public RelayCommand<object> Insert => new RelayCommand<object>((obj) => ExecuteInsert(obj));
        public RelayCommand<object> Delete => new RelayCommand<object>((obj) => ExecuteDelete(obj));
        #endregion
        #region Command Methods
        private void ExecuteSearch(object obj)
        {
            if (obj == null)
            {
                this.GetAllEmployees();
            }
            else
            {
                var fileterEmployee = from item in AllEmployees where item.Name == obj.ToString() select item;
                DisplayInfo = new ObservableCollection<Employee>(fileterEmployee);
            }
        }
        private void ExecuteExport()
        {
            if (MainSer.ExportAsExcel(DisplayInfo.ToList()))
            {
                System.Windows.MessageBox.Show("导出成功!", "系统提示");
            }
            else
            {
                System.Windows.MessageBox.Show("导出失败", "系统提示");
            }
        }
        private void ExecuteUpdate(object obj)
        {
            if(obj is Employee em)
            {
                using (SuperManagerEntities context = new SuperManagerEntities())
                {
                    Employee employee= context.dtEmployee.Find(em.ID);
                    //更新数据信息
                    employee.Name = em.Name;
                    employee.Phone = em.Phone;
                    employee.Remark = em.Remark;
                    employee.Address = em.Address;
                    employee.Alternate = em.Alternate;
                    employee.Education = em.Education;
                    employee.Email = em.Email;
                    context.SaveChanges();
                    MessageBox.Show("更新成功", "系统提示");
                }
            }
            
        }
        private void ExecuteInsert(object obj)
        {
            if(obj is Employee em)
            {
                using (SuperManagerEntities context = new SuperManagerEntities())
                {
                    context.dtEmployee.Add(em);
                    context.SaveChanges();
                    DisplayInfo.Add(em);
                    MessageBox.Show("插入成功", "系统提示");

                }
            }
        }
        public void ExecuteDelete(object obj)
        {
            if(obj is Employee em)
            {
                using (SuperManagerEntities context = new SuperManagerEntities())
                {
                    Employee employee = context.dtEmployee.Find(em.ID);
                    context.dtEmployee.Remove(employee);
                    context.SaveChanges();
                    DisplayInfo.Remove(em);
                    System.Windows.MessageBox.Show("删除成功", "系统提示");
                }
            }
        }
        #endregion
        #region Methods
        private void GetAllEmployees()
        {
            using (SuperManagerEntities context = new SuperManagerEntities())
            {
                AllEmployees = (from item in context.dtEmployee select item).ToList<Employee>();
                Name = (from item in AllEmployees select item.Name).ToList<string>();
                DisplayInfo = new ObservableCollection<Employee>(AllEmployees);
            }
        }
        #endregion
    }
}
