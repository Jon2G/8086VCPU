using Kit.Model;

namespace _8086VCPU.Alu
{
    public class Banderas : ModelBase
    {
        private bool _Carry;
        public bool Carry { get => _Carry; set { _Carry = value; OnPropertyChanged(); } }
        private bool _Signo;
        public bool Signo { get => _Signo; set { _Signo = value; OnPropertyChanged(); } }
        private bool _Zero;
        public bool Zero { get => _Zero; set { _Zero = value; OnPropertyChanged(); } }
        private bool _OverFlow;
        public bool OverFlow { get => _OverFlow; set { _OverFlow = value; OnPropertyChanged(); } }

        internal void Clear()
        {
            Carry = false;
            Signo = false;
            Zero = false;
            OverFlow = false;
        }
    }
}
