using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.IO;

namespace Loader_Manager_App.Clases
{
    public class Licencia
    {
        //// Devuelve el código interno de la máquina
        //public static string dame_codigo_id_maquina()
        //{
        //    string cpuInfo = string.Empty;
        //    ManagementClass mc = new ManagementClass("win32_processor");
        //    ManagementObjectCollection moc = mc.GetInstances();
        //    foreach (ManagementObject mo in moc)
        //    {
        //        cpuInfo = mo.Properties["processorID"].Value.ToString();
        //        break;
        //    }
        //    return cpuInfo;
        //}

        // Encripta una cadena
        public string encripta(string cadena_a_encriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadena_a_encriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        // Esta función desencripta la cadena que le envíamos en el parámentro de entrada       
        public string desencripta(string cadena_a_desencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(cadena_a_desencriptar);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }


        
        public string dame_id_maquina()
        {
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("CMD.EXE", "/C wmic csproduct get uuid");
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd();
            int pos = result.IndexOf("\n");
            string aux = result.Substring(pos + 1, result.Length - pos - 1);

            proc.WaitForExit();
            proc.Close();

            return aux;
        }


        // Return = "2": id de la máquina distinto
        // Return = "3": producto incorrecto
        // Return = "4": licencia expirada
        // Return = "5": licencia correcta
        private string comprueba_licencia(string id_maquina, string producto, string fecha_expiracion)
        {
            string idMachineDecryp = "";
            int posSpace = -1;

            // Get el id de la máquina con el comando de msdos
            idMachineDecryp = dame_id_maquina();

            DateTime dt1 = DateTime.Now;
            DateTime dt2 = Convert.ToDateTime(fecha_expiracion);
            int result = DateTime.Compare(dt1, dt2);                     

            posSpace = fecha_expiracion.IndexOf(' ');
            if (idMachineDecryp.Trim() != id_maquina.Trim())
                return "2";
            else
            {
                if (producto != "DEPORTES SETUP APP")
                    return "3";
                else
                {
                    if (result > 0)
                        return "4";
                    else
                        return fecha_expiracion.Substring(0, posSpace).Trim();
                }
            }
        }


        // Return = "#ERROR 01": El fichero "serial.lic" no se encuentra en el directorio. 
        // Return = "#ERROR 02": Identificador de la máquina del fichero es el mismo que si ejecutamos el comando "wmic csproduct get uuid".
        // Return = "#ERROR 03": Producto incorrecto.
        // Return = "#ERROR 04": Licencia expirada.        
        // Return = "#ERROR 05": Que la fecha del ordenador en el momento de la ejecución de la aplicación
        // es mayor o igual que la fecha que viene como último parámetro. Si esta condición se cumple, actualizaremos este último parámetro del 
        // fichero "serial.lic", de manera que lo que conseguimos es tener almacenado la fecha encriptada de la última ejecución. De esta manera,
        // si en un momento dado la fecha del ordenador es menor que la fecha almacenada en este parámetro significa que el usuario ha cambiado
        // a mano la fecha del ordenador, y por tanto, la licencia no sería correcta.
        // Return = "#ERROR 06": Queda menos de una semana para la fecha de expiración.
        public string dame_licencia()
        {
            string id_maquina_en_fichero = "", producto = "", fecha_expiracion = "", licencia = "", ultima_ejecucion = "";
            string path_fichero_licencia = ".\\serial.lic";
            string[] words = new string[4];
            DateTime dt1 = DateTime.Now;
            DateTime dt2;

            if (!File.Exists(path_fichero_licencia))
                return "Producto no licenciado. Código ERROR #01";
            else
            {
                try
                {
                    using (StreamReader sr = new StreamReader(path_fichero_licencia))
                    {
                        licencia = sr.ReadToEnd();
                        sr.Close();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("The file could not be read: ");
                    Console.WriteLine(exception.Message);
                }
                char[] delimiterChars = { ';' };
                words = licencia.Split(delimiterChars);
                id_maquina_en_fichero = desencripta(words[0]);
                producto = desencripta(words[1]);
                fecha_expiracion = desencripta(words[2]);
                ultima_ejecucion = desencripta(words[3]);

                //Console.WriteLine(idMachineOnFile);
                //Console.WriteLine(product);
                //Console.WriteLine(expirationDate);

                // Comprobamos si han cambiado la fecha del reloj del sistema
                dt1 = DateTime.Now;
                dt2 = Convert.ToDateTime(ultima_ejecucion);
                int result = DateTime.Compare(dt1, dt2);
                if (result <= 0)
                    return "Producto no licenciado. Código ERROR #05";

                string datos_licencia = comprueba_licencia(id_maquina_en_fichero, producto, fecha_expiracion);
                if ((datos_licencia == "2") || (datos_licencia == "3") || (datos_licencia == "4"))
                    return "Producto no licenciado. Código ERROR #0" + datos_licencia;
                else
                {
                    // Comprobamos si queda menos de una semana para que expire la licencia
                    dt2 = Convert.ToDateTime(fecha_expiracion);
                    TimeSpan ts = dt2 - dt1;
                    int differenceInDays = ts.Days;
                    if (differenceInDays <= 7)
                        return "Producto licenciado hasta " + datos_licencia + ". [ ¡¡ ATENCIÓN !! MENOS DE 7 DÍAS ]";
                    else
                        return "Producto licenciado hasta " + datos_licencia;
                }
            }
        }
    }
}
