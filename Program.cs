using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Base_de_datos_generica
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             Ejercicio: Base de Datos Genérica

             Crear una clase genérica llamada BaseDeDatos<T> que simule una 
             base de datos simple para almacenar elementos de tipo T. La base de datos 
             debe permitir realizar operaciones de inserción, eliminación, búsqueda y 
             recuperación de elementos.

             La clase debe tener una lista interna que almacene los elementos de tipo T.     

             Implementa un método Insertar que permita agregar un elemento de tipo T a la base de datos. 
            
             Implementa un método Eliminar que permita eliminar un elemento de tipo T de la base de datos.

             Implementa un método Buscar que permita buscar elementos en la base de datos
             y devolver una lista de elementos que cumplan ciertos criterios.

             Implementa un método Recuperar que permita recuperar todos los elementos de la base de datos
             osea devolver la lista actual y mostrarla.
             */


            //Variable
            int op = 0;


            //Creamos 3 listas, una con sin tamaño y otras con 4 elementos de tamaño
            List<int> listInts = new();
            List<string> listStrings = new(4);
            List<char> listChars = new(4);


            //Instanciamos
            Database<int> databaseInts = new(listInts);
            Database<string> databaseStrings = new(listStrings);
            Database<char> databaseChars = new(listChars);


            //Aplicamos metodos con enteros
            databaseInts.Delete(4);

            //Insertamos
            databaseInts.Insert(5);
            databaseInts.Insert(4);
            databaseInts.Insert(3);
            databaseInts.Insert(8);
            databaseInts.Insert(10);
            databaseInts.Insert(12);

            //Eliminamos un elemento
            databaseInts.Delete(3);


            Console.WriteLine();

            //Mostramos lista
            databaseInts.Show(databaseInts.GetList());


            //Filtramos lista
            List<int> listFiltered = databaseInts.Search(op);


            Console.WriteLine();

            //Mostramos lista
            databaseInts.Show(listFiltered);


            Console.WriteLine();


            //Aplicamos metodos con strings
            databaseStrings.Delete(3);


            //Insertamos
            databaseStrings.Insert("las");
            databaseStrings.Insert("hora de aventura");
            databaseStrings.Insert("pedro");
            databaseStrings.Insert("alo");
            databaseStrings.Insert("Holaa");
            databaseStrings.Insert("ene");


            //Eliminamos un elemento
            databaseStrings.Delete(4);


            Console.WriteLine();

            //Mostramos la lista 
            databaseStrings.Show(databaseStrings.GetList());


            Console.WriteLine();

            //Filtramos lista
            List<string> listFiltered2 = databaseStrings.Search(op);


            //Mostramos lista
            databaseStrings.Show(listFiltered2);

            Console.WriteLine();

            //Probamos con chars
            databaseChars.Delete(3);

            //Insertamos
            databaseChars.Insert('s');
            databaseChars.Insert('X');
            databaseChars.Insert('A');
            databaseChars.Insert('e');
            databaseChars.Insert('d');
            databaseChars.Insert('m');

            //Eliminamos un elemento
            databaseChars.Delete(4);

            Console.WriteLine();

            //Mostramos lista
            databaseChars.Show(databaseChars.GetList());


            Console.WriteLine();

            //Filtramos lista
            List<char> listFiltered3 = databaseChars.Search(op);

            Console.WriteLine();

            //Mostramos lista
            databaseChars.Show(listFiltered3);

            Console.ReadKey();
        }
    }


    //Clase
    class Database<T>
    {

        //Propiedades
        private List<T> list;



        //Constructor
        public Database(List<T> list)
        {
            this.list = list;
        }




        //Metodos:



        //Insertar elemento
        public void Insert(T element)
        {

            //Verificamos el tipo de dato
            if (element?.GetType() == typeof(T))
            {
                //Verificamos si la lista esta llena
                if (list.Count > list.Capacity)
                {
                    Console.WriteLine("Error, la lista está llena.");
                }
                else
                {
                    list.Add(element);
                    Console.WriteLine($"El elemento {element} se añadió exitosamente.");
                }
            }
            else
            {
                throw new InvalidCastException($"Error, el elemento {element} es inválido.");
            }
        }



        //Eliminar elemento
        public void Delete(int index)
        {

            //Verificamos el indice
            if (list.Count == 0) Console.WriteLine("Error, la lista está vacía.");
            else if (index < 0 || index >= list.Count) Console.WriteLine("Error, el índice es inválido.");
            else list.RemoveAt(index);

        }



        //Metodo para encapsular la logica de buscar elementos
        public List<T> Search(int op)
        {

            //Pedimos una opcion
            RequestOption(ref op);


            //Retornamos el resultado de la busqueda
            return ApplySearch(op);

        }




        //Metodo para pedir metodo de busqueda
        public void RequestOption(ref int op)
        {

            //Mostramos metodos de busqueda
            DisplaySearchMethods();

            //Pedimos opcion
            op = GetValidOp("Digite una opcion: ");

        }



        //Metodo para aplicar el metodo de busqueda
        public List<T> ApplySearch(int op)
        {


            //Verificamos si es null o esta vacia
            if (list == null || list.Count == 0) return new List<T>();
            else
            {
                //Creamos lista
                List<T> resultado = null;


                //Verificamos tipo de dato y aplicamos filtros
                switch (list[0]?.GetType())
                {
                    case Type intType when intType == typeof(int) || intType == typeof(double):
                        resultado = HandleNumericSearch(op);
                        break;

                    case Type stringType when stringType == typeof(string):
                        resultado = HandleStringSearch(op);
                        break;

                    case Type charType when charType == typeof(char):
                        resultado = HandleCharSearch(op);
                        break;

                    default:
                        throw new InvalidOperationException("Tipo de dato no admitido.");
                }


                //Retornamos el resultado
                return resultado;

            }
        }


        //Metodo para recuperar lista
        public List<T> GetList()
        {
            return list;
        }



        //Metodo para mostrar lista
        public void Show(List<T> list)
        {

            //Verificamos la lista antes de mostrarla
            if (list == null)
            {
                Console.WriteLine("La lista está vacía.");
                return;
            }

            Console.Write("La lista es: ");
            list.ForEach(element => Console.Write(element + " "));
            Console.WriteLine();

        }



        //Metodo auxiliar para mostrar menu de metodos
        public void DisplaySearchMethods()
        {

            Console.WriteLine("Metodos de busqueda: ");

            if (typeof(T) == typeof(int) || typeof(T) == typeof(double))
            {
                Console.WriteLine("1: Buscar pares");
                Console.WriteLine("2: Buscar impares");
                Console.WriteLine("3: Buscar numeros mayores a un numero");
            }
            else if (typeof(T) == typeof(string))
            {
                Console.WriteLine("1: Buscar por longitud mayor que un numero");
                Console.WriteLine("2: Buscar por longitud menor que un numero");
                Console.WriteLine("3: Buscar cadena especifica");
            }
            else if (typeof(T) == typeof(char))
            {
                Console.WriteLine("1: Buscar caracter especifico");
                Console.WriteLine("2: Buscar mayusculas");
                Console.WriteLine("3: Buscar minusculas");

            }
        }




        //Metodo auxiliar para validar opcion
        public static int GetValidOp(string message)
        {
            //variable a usar
            int opcion;
            do
            {
                Console.Write(message);

                //se repite hasta que se digite un int o sea mayor a 4 o menor que 1
            } while (!int.TryParse(Console.ReadLine(), out opcion) || opcion <= 0 || opcion > 3);

            return opcion;
        }



        //Metodo auxiliar para validar numero
        public static int GetValidValue(string message)
        {
            //variable a usar
            int value;

            do
            {
                Console.Write(message);

                //se repite hasta que se digite un int o sea mayor a 4 o menor que 1
            } while (!int.TryParse(Console.ReadLine(), out value));


            return value;
        }



        //Metodo para validar valores genericos
        public static T GetValidValue<T>(string message) where T : IConvertible
        {
            T? value = default;

            do
            {
                Console.Write(message);

                string? input = Console.ReadLine();

                try
                {
                    value = (T)Convert.ChangeType(input, typeof(T));
                }
                catch
                {
                    Console.WriteLine("Entrada no válida. Intente nuevamente.");
                }

            } while (!IsValidValue(value));

            return value;
        }





        //Para verificar el tipo de dato
        private static bool IsValidValue<T>(T value) where T : IConvertible
        {

            //En caso de que sea int se verifica si es mayor que 0
            if (typeof(T) == typeof(int))
            {
                int intValue = Convert.ToInt32(value);
                return intValue > 0;
            }
            //En caso de que sea int se verifica si esta vacia
            else if (typeof(T) == typeof(string))
            {
                string? stringValue = value.ToString();
                return !string.IsNullOrEmpty(stringValue);
            }
            //En caso de que sea int se verifica que sea letra xd
            else if (typeof(T) == typeof(char))
            {
                char charValue = Convert.ToChar(value);
                return char.IsLetter(charValue);
            }

            return true;
        }




        //Metodos auxiliares para encapsular las busquedas
        private List<T> HandleNumericSearch(int op)
        {
            List<T> result = new List<T>();

            switch (op)
            {
                case 1:
                    result = list.FindAll(n => (n is int intValue && intValue % 2 == 0) || (n is double doubleValue && doubleValue % 2 == 0));
                    break;

                case 2:
                    result = list.FindAll(n => (n is int intValue && intValue % 2 != 0) || (n is double doubleValue && doubleValue % 2 != 0));
                    break;

                case 3:
                    int targetValue = GetValidValue<int>("Digite un número: ");
                    result = list.FindAll(n => n is int intValue && intValue > targetValue);
                    break;

                default:
                    break;
            }

            return result;
        }




        private List<T> HandleStringSearch(int op)
        {
            List<T> result = new List<T>();

            switch (op)
            {
                case 1:
                    int minLength = GetValidValue<int>("Digite un número: ");
                    result = list.FindAll(n => n is string stringValue && stringValue.Length >= minLength);
                    break;

                case 2:
                    int maxLength = GetValidValue<int>("Digite un número: ");
                    result = list.FindAll(n => n is string stringValue && stringValue.Length <= maxLength);
                    break;

                case 3:
                    string targetString = GetValidValue<string>("Digite una cadena: ");
                    result = list.FindAll(n => n is string stringValue && stringValue == targetString);
                    break;

                default:
                    break;
            }

            return result;
        }



        private List<T> HandleCharSearch(int op)
        {
            List<T> result = new List<T>();

            switch (op)
            {
                case 1:
                    char targetChar = GetValidValue<char>("Digite una letra: ");
                    result = list.FindAll(n => n is char charValue && charValue == targetChar);
                    break;

                case 2:
                    result = list.FindAll(n => n is char charValue && char.IsUpper(charValue));
                    break;

                case 3:
                    result = list.FindAll(n => n is char charValue && char.IsLower(charValue));
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}