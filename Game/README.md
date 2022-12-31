# SlotMachine
Um jogo estilo Slots com tema natalino

## Tela inicial

<img src="ReadMeImages/SlotMachine%20Menu.jpg" alt="SlotMachine Menu" style="height: 300px; width:233px;"/>
    
Nesta tela de menu, o jogador inicia o jogo clicando no botão <b>Begin</b>.

## Os slots

<img src="ReadMeImages/SlotMachine%20running.jpg" alt="SlotMachine Menu" style="height: 300px; width:480px;"/>

Nesta tela o jogador começará a jogatina.

Ao realizar uma jogada, será enviado ao servidor uma requisição contento o valor da aposta e este retornará a matriz de valores que irão compor os elementos em tela. Cada elemento (num total de 11) possui uma pontuação atrelada a ele.

Para marcar a pontuação, é necessário tirar no mínimo 3 elementos iguais na tela.

## Game Over

<img src="ReadMeImages/SlotMachine%20gameover.jpg" alt="SlotMachine Menu" style="height: 300px; width:336 px;"/>

Esta tela aparece quando a última jogada deixa o saldo com valor menor que a menor aposta. Então o jogador pode apenas voltar ao menu.
