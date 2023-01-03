from flask import Flask, request
from slot_machine import SlotMachine
import json


machine = SlotMachine(3, 5, 12)


app = Flask(__name__)


@app.route("/bet", methods=["POST"])
def post_bet():
  received_bet = int(list(request.form.keys())[0])
  machine.bet = received_bet
  machine.credits -= machine.bet
  return "201", 201


@app.route("/betlist")
def get_betlist():
  return json.dumps(machine.bet_list)


@app.route("/start")
def get_start():
  machine.start_structure()
  return machine.slot_machine_data()

@app.route("/ordered")
def get_ordered():
  machine.ordered_structure()
  if machine.bet < machine.bet_list[0]:
    safety_game_over()
  return machine.slot_machine_data()


@app.route("/random")
def get_random():
  machine.random_structure()
  print(f"bet: {machine.bet}  < smaller_bet: {machine.bet_list[0]} ? {machine.bet < machine.bet_list[0]}")
  if machine.bet < machine.bet_list[0]:
    safety_game_over()
  return machine.slot_machine_data()


@app.route("/gameover")
def get_menu():
  machine.game_over()
  return machine.slot_machine_data()


@app.route("/")
def get_ready():
  return "ready"


def safety_game_over():
  machine.game_over()
  return app.redirect("/gameover", 302)


if __name__ == "__main__":
  app.run(host="0.0.0.0", debug=True)
