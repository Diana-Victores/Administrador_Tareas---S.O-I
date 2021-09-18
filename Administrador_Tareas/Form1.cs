using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

//********************** ESTE PROYECTO FUE REALIZADO GRUPALMENTE 
//****************************************** DIANA VICTORES SIMULADOR/ ADMINISTRADOR DE TAREAS ********************************
namespace Simulacion_Procesos 
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        //Declaracion de variable string para obtener el nombre del proceso en la tabla para su eliminacion
        string Str_Obt_Proc;
        public Form1()
        {
            InitializeComponent();
            //Activacion del timer que actualizara la tabla
            ActualizarTabla();
            timer1.Enabled = true;
            
        }

        //****************************************Actualizar Tabla - DIANA VICTORES ************************************
        // USO DE REFERENCIAS 
       
        private void ActualizarTabla() 
        {
            //LIMPIEZA DEL DATAGRID
            dgv_Proceso.Rows.Clear();

            //*************************************CREACION DE TABLAS CON SUS  COLUMNAS RESPECTIVAS ***********************************
            dgv_Proceso.Columns[0].Name = "Num. Procesos";
            dgv_Proceso.Columns[1].Name = "Procesos";
            dgv_Proceso.Columns[2].Name = "Prioridad Proceso";
            dgv_Proceso.Columns[3].Name = "ID";
            dgv_Proceso.Columns[4].Name = "Memoria Fisica";
            dgv_Proceso.Columns[5].Name = "Memoria Virtual";
           

            //Propiedad para autoajustar el tamaño de las celdas segun su contenido
            dgv_Proceso.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //Propiedad para que el usuario seleccione solamente filas en la tabla y no celdas sueltas
            dgv_Proceso.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //Propiedad para que el usuario no pueda seleccionar mas de una fila
            dgv_Proceso.MultiSelect = false;

            //Declaracion de la variable que sera un contador para el total de procesos
            int Int_Cant_Proc = 1;

            //************************************************* CARLOS FLORES *****************************************

            foreach (Process Proc_Proceso in Process.GetProcesses()) 
            {
                //Ingreso de los datos en el datagrid
                //Se utilizo datagrid para mostrar la cantidad de procesos que hay en uso de la computadora
                dgv_Proceso.Rows.Add(Int_Cant_Proc, Proc_Proceso.ProcessName, Proc_Proceso.BasePriority, Proc_Proceso.Id, Proc_Proceso.WorkingSet64,
                    Proc_Proceso.VirtualMemorySize64);

                //Aumento en 1  variable
                Int_Cant_Proc += 1;
            }
            //El label muestra la cantidad de procesos actuales
            lbl_Contador.Text = "Procesos Actuales: " + (Int_Cant_Proc - 1);     //Cantidad de procesos   


         } 


        //******************************* Boton Actualizar - RITA SIPAQUE ******************************
        private void BtnActualizar_Click(object sender, EventArgs e){

            //Ocultamos todos los objetos para los graficos y mostramos solo el Dgv de Procesos,
            //mostramos solo la tabla de procesos 

            LblNombreCPU.Visible = false;
            LblNombreRam.Visible = false;
            ProgressBarCPU.Visible = false;
            ProgressBarRAM.Visible = false;
            LblPorCPU.Visible = false;
            LblPorRAM.Visible = false;
            Grafico.Visible = false;
            dgv_Proceso.Visible = true;

            //Llamado al proceso para actualizar la tabla
            ActualizarTabla(); 
        }



        //************************************ Boton Detener Proceso - CARLOS MONTES *******************************
        private void Btn_Detener_Click(object sender, EventArgs e){

            //Ocultamos todos los objetos para los graficos y mostramos solo el Dgv de Procesos,
            //mostramos que al detener el proceso y seleccionamos un elemento
            //por ejemplo la calculadora y damos detener proceso se elimina
            LblNombreCPU.Visible = false;
            LblNombreRam.Visible = false;
            ProgressBarCPU.Visible = false;
            ProgressBarRAM.Visible = false;
            LblPorCPU.Visible = false;
            LblPorRAM.Visible = false;
            Grafico.Visible = false;
            dgv_Proceso.Visible = true;

            try 
            {
                //Por cada proceso que haya se comparara su nombre con el nombre del proceso que se desea eliminar.
                foreach(Process proceso in Process.GetProcesses())
                {
                    if (proceso.ProcessName == Str_Obt_Proc)
                    {
                        proceso.Kill();//Se elimina el proceso
                        ActualizarTabla();
                        //Seleccionamos actualizar tabla nuevamente y
                        //se llama al proceso para actualizar la tabla automaticamente
                    }
                }

            }
                //En caso no de no seleccionar un proceso mostrara el siguiente mensaje.
            catch (Exception x) 
            {
                MessageBox.Show("No se ha seleccionado ningun proceso para detener." + x, "Error al Detener Proceso", MessageBoxButtons.OK);
            }
        }

        //****************************************************************************************
        private void dgv_Proceso_MouseClick_1(object sender, MouseEventArgs e)
        {
            //La variable obtiene el Nombre del Proceso de la Tabla al hacerle clic
            Str_Obt_Proc = dgv_Proceso.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void Form1_Load(object sender, EventArgs e){
            //Evento que maneja el contador de rendimiento de la RAM y del CPU
            timer.Start();
        }
        

        private void Timer_Tick(object sender, EventArgs e){  //-

            //Asignamos un float a un PerformanceCounter que ya tiene
            //asignado el contador de rendimiento de la RAM y del CPU
            float fCPU = pCPU.NextValue();
            float fRAM = pRAM.NextValue();

            //Agregamos los Valores a la Barra de progeso respectivamente
            ProgressBarCPU.Value = (int)fCPU;
            ProgressBarRAM.Value = (int)fRAM;

            //Damos el formato de porcentaje correspondiente al label de porcentaje con lo float
            LblPorCPU.Text = string.Format("{0:0.00}%", fCPU);
            LblPorRAM.Text = string.Format("{0:0.00}%", fRAM);

            //Agregamos los valores de Y que se usaran para mostrarlos en grafica
            Grafico.Series["CPU"].Points.AddY(fCPU);
            Grafico.Series["RAM"].Points.AddY(fRAM);
        }

        private void Button1_Click(object sender, EventArgs e){ //- 
            //Ocultamos el Dgv de Procesos y mostramos todos los objetos para los gráficos
            LblNombreCPU.Visible = true;
            LblNombreRam.Visible = true;
            ProgressBarCPU.Visible = true;
            ProgressBarRAM.Visible = true;
            LblPorCPU.Visible = true;
            LblPorRAM.Visible = true;
            Grafico.Visible = true;
            dgv_Proceso.Visible = false;
        }

        private void Grafico_Click(object sender, EventArgs e)
        {

        }
    }
}
