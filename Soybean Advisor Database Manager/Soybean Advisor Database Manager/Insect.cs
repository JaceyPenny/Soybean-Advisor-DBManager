using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Soybean_Advisor_Database_Manager
{
    [ParseClassName("Insect")]
    public class Insect : ParseObject, Pictureable
    {
        [ParseFieldName("insectId")]
        public int InsectId
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

        [ParseFieldName("description")]
        public string Description
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

        [ParseFieldName("insecticideIds")]
        public IList<int> InsecticideIds
        {
            get
            {
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

        public Insect()
        {

        }

        int Pictureable.GetPictureID()
        {
            return PictureId;
        }
    }
}
