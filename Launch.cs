using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirScanner
{
    public partial class Launch : Form
    {
        public Launch()
        {
            InitializeComponent();
            this.Cursor = Cursors.WaitCursor;
        }

        private void Launch_Load(object sender, EventArgs e)
        {
            // this.Hide();
            // this.Visible = false;
           
            AnimateLauncher();
           // GotoScreen((int)Tools.FORMS.login, this);
            
        }
        private async void AnimateLauncher()
        {

            progressBar.Show();

            //move x little by little to match the progressbar
            for (int x = 0; x < progressBar.Maximum; x++)
            {              
               await Task.Delay(TimeSpan.FromSeconds(0.0005));
                progressBar.Value += 1;
            }
            //gives the program time to display the meter fully
            await Task.Delay(TimeSpan.FromSeconds(.75));
            GotoScreen(2, this);           
        }

        
        //***********************************************************************//
        //Screen GUI and transition functions
        //***********************************************************************//
        public static void GotoScreen(int screen, Form ThisScreen)
        {
            //as long as the enum int value isn't the Last one/max, transition. 
            if (screen < 3)
            {
                Form NextScreen = AppScreen(screen);
                // nextScreen.Copy(thisScreen);

                ThisScreen.Hide();
                NextScreen.Show();
                ThisScreen.Cursor = Cursors.Default;
            }
        }

        //Given an int value for the screen, use the tools.FORMS enum to transition to the corresponding screen
        public static Form AppScreen(int screen)
        {
            Form form = new Form();
            switch (screen)
            {
                //creates a form out of the given int screen
                case 1: form = new Launch(); break;
                case 2: form = new GUI(); break;                   
            }
            
            //returns the given form to transition to
            return form;
        }
    }
}
