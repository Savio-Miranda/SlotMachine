from flask import Flask, request
from slot_machine import SlotMachine
import json


machine = SlotMachine(3, 5, 11)


app = Flask(__name__)


@app.route("/bet", methods=["POST"])
def post_bet():
  # if request.method == "POST":
  tudo_isso_pra_chegar_num_numero = int(list(request.form.keys())[0])
  machine.bet = tudo_isso_pra_chegar_num_numero
  machine.credits -= machine.bet
  return "201"


@app.route("/betlist")
def get_betlist():
  return json.dumps(machine.bet_list)


@app.route("/start")
def get_start():
  machine.start_structure()
  return machine.Json_Repr()

@app.route("/ordered")
def get_ordered():
  machine.ordered_structure()
  return machine.Json_Repr()


@app.route("/random")
def get_random():
  machine.random_structure()
  return machine.Json_Repr()


@app.route("/gameover")
def get_menu():
  machine.Game_Over()
  return machine.Json_Repr()


@app.route("/")
def get_ready():
  return "ready"


if __name__ == "__main__":
  app.run(host="0.0.0.0")
