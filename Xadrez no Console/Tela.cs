using System;
using tabuleiro;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xadrez_no_Console {
    class Tela {
        public static void imprimirTabuleiro(Tabuleiro tab) {
            for (int i = 0; i < tab.linhas; i++) {
                for (int j = 0; j < tab.colunas; j++) {
                    if (tab.peca(i, j) == null) {
                        Console.Write("- ");
                    }
                    else
                        Console.Write(tab.peca(i, j) + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
