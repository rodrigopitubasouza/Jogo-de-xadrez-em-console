using System;
using System.Collections.Generic;
using tabuleiro;

namespace xadrez {
    class PartidaDeXadrez {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> pecasCapturadas;


        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            pecas = new HashSet<Peca>();
            pecasCapturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca) {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas() {
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            /* colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Rainha(tab, Cor.Branca));*/
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branca));
            /* colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
             colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));*/
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            /*colocarNovaPeca('a', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branca, this));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branca, this));*/

            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            /*colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Rainha(tab, Cor.Preta));*/
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preta));
            /*   colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
               colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));*/
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            /*  colocarNovaPeca('a', 7, new Peao(tab, Cor.Preta, this));
              colocarNovaPeca('b', 7, new Peao(tab, Cor.Preta, this));
              colocarNovaPeca('c', 7, new Peao(tab, Cor.Preta, this));
              colocarNovaPeca('d', 7, new Peao(tab, Cor.Preta, this));
              colocarNovaPeca('e', 7, new Peao(tab, Cor.Preta, this));
              colocarNovaPeca('f', 7, new Peao(tab, Cor.Preta, this));
              colocarNovaPeca('g', 7, new Peao(tab, Cor.Preta, this));
              colocarNovaPeca('h', 7, new Peao(tab, Cor.Preta, this));*/
        }


        public void realizaJogada(Posicao origem, Posicao destino) {
            executaMovimento(origem, destino);
            turno++;
            mudaJogador();

        }

        private void executaMovimento(Posicao origem, Posicao destino) {

            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null) {
                pecasCapturadas.Add(pecaCapturada);
            }
        }

        public HashSet<Peca> pecasCapturadasPorCor(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecasCapturadas) {
                if (x.cor == cor) {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas) {
                if (x.cor == cor) {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadasPorCor(cor));
            return aux;
        }

        private void mudaJogador() {
            if (jogadorAtual == Cor.Branca)
                jogadorAtual = Cor.Preta;
            else
                jogadorAtual = Cor.Branca;
        }

        public void validarPosicaoOrigem(Posicao pos) {
            if (tab.peca(pos) == null) {
                throw new tabuleiroException("Não existe peça na posição de origem escolhida");
            }
            if (jogadorAtual != tab.peca(pos).cor) {
                throw new tabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis()) {
                throw new tabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDestino(Posicao pos, bool[,] posicoesPossiveis) {
            if (!tab.posicaoValida(pos)) {
                throw new tabuleiroException("Não existe a posição digitada");
            }
            if (!posicoesPossiveis[pos.linha, pos.coluna]) {
                throw new tabuleiroException("Posição de destino inválida");
            }
        }

        
    }
}
