using System;
using tabuleiro;
using xadrez;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xadrez_no_Console {
    class Program {
        static void Main(string[] args) {
            try {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.colocarPeca(new Torre(tab, Cor.Amarela), new Posicao(0, 0));
                tab.colocarPeca(new Torre(tab, Cor.Amarela), new Posicao(0, 0));
                Tela.imprimirTabuleiro(tab);
            }
            catch (tabuleiroException e) {
                PosicaoXadrez x = new PosicaoXadrez('c',7);
                Console.WriteLine(x.toPosicao());
                Console.WriteLine(e.Message);
            }

            
            Console.ReadLine();
        }
    }
}
