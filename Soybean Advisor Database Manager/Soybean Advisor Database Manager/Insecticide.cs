using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Soybean_Advisor_Database_Manager
{
    [ParseClassName("Insecticide")]
    public class Insecticide : ParseObject
    {
        [ParseFieldName("insecticideId")]
        public int InsecticideId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("other")]
        public string OtherName
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("acres")]
        public string Acres
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("active")]
        public string Active
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("comments")]
        public string Comments
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("formulation")]
        public string Formulation
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("entryInterval")]
        public int EntryInterval
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("minimum")]
        public int MinimumDays
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("performance")]
        public int Performance
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("restricted")]
        public bool Restricted
        {
            get { return GetProperty<bool>(); }
            set { SetProperty<bool>(value); }
        }

        public Insecticide()
        {

        }

        public override string ToString()
        {
            return "Insecticide " + ObjectId + ", InsecticideId = " + InsecticideId + ", Name = " + Name
                + ", OtherName = " + OtherName + ", Acres = " + Acres + ", Active = " + Active + ", Comments = "
                    + Comments.Substring(0,10) + "... , Formulation = " + Formulation + ", EntryInterval = "
                    + EntryInterval + ", MinimumDays = " + MinimumDays + ", Performance = " + Performance
                    + ", Restricted = " + ((Restricted)?true:false);
        }
    }
}
