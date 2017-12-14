using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabuleiro {
   abstract class Peca {
        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int qtdMovimento { get; protected set; }
        public Tabuleiro tab { get; protected set; }

        public Peca(Cor cor, Tabuleiro tab) {
            posicao = null;
            this.cor = cor;
            this.tab = tab;
            qtdMovimento = 0;
        }

        public void incrementarQtdMovimentos() {
            qtdMovimento++;
        }

        public void decrementaQtdMovimentos() {
            qtdMovimento--;
        }

        public bool existeMovimentosPossiveis() {
            bool[,] mat = movimentosPossiveis();
            for (int i = 0; i < tab.linhas; i++) {
                for(int j = 0; j< tab.colunas; j++) {
                    if (mat[i, j])
                        return true;
                }
            }
            return false;
        }


        public abstract bool[,] movimentosPossiveis();

        
    }
}
