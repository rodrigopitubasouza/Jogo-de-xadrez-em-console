using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabuleiro {
    class tabuleiroException : Exception{
        public tabuleiroException(String msg) : base(msg) {
        }
    }
}
