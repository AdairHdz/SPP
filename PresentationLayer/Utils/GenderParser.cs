using DataPersistenceLayer.Entities;
using System.Windows.Controls;

namespace PresentationLayer.Utils
{
    public static class GenderParser
    {
        public static Gender ParseFromRadioButtonsToObject(RadioButton maleRadioButton)
        {
            Gender gender;
            if (maleRadioButton.IsChecked == true)
            {
                gender = Gender.MALE;
            }
            else
            {
                gender = Gender.FEMALE;
            }
            return gender;
        }
        
    }
}
