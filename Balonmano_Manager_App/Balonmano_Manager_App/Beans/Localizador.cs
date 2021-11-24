using System;

namespace Balonmano_Manager_App.Beans
{
    [Serializable]
    public class Localizador
    {
        public Localizador(string title, string localizador)
        {
            Title = title;
            TextoLocalizador = localizador;
        }
        public string Title { get; set; }

        public string TextoLocalizador { get; set; }
    }
}
