using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Soybean_Advisor_Database_Manager
{
    [ParseClassName("Disease")]
    public class Disease : ParseObject, Pictureable
    {
        [ParseFieldName("diseaseId")]
        public int DiseaseId
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

        [ParseFieldName("category")]
        public string Category
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("symptoms")]
        public string Symptoms
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("pictureId")]
        public int PictureId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("controlIds")]
        public IList<int> ControlIds
        {
            get {
                if (GetProperty<IList<int>>() == null)
                {
                    return new List<int>();
                    
                }
                else
                {
                    return GetProperty<IList<int>>(); 
                }
            }
            set { SetProperty<IList<int>>(value); }
        }

        [ParseFieldName("management")]
        public IList<string> Management
        {
            get 
            {
                if (GetProperty<IList<string>>() == null)
                {
                    return new List<string>();
                }
                return GetProperty<IList<string>>();
            }
            set { SetProperty<IList<string>>(value); }
        }

        public Disease()
        {

        }

        int Pictureable.GetPictureID()
        {
            return PictureId;
        }
    }
}
