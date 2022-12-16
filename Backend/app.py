from flask import Flask
from send_matrix import Matrix
import json

matrix = Matrix(3, 5, 11)
app = Flask(__name__)


@app.route("/betlist")
def get_betlist():
  return json.dumps(matrix.bet_list)


@app.route("/ordered")
def get_ordered():
  matrix.ordered_structure()
  return json.dumps(matrix.current_matrix.tolist())


@app.route("/random")
def get_random():
  matrix.random_structure()
  return json.dumps(matrix.current_matrix.tolist())


@app.route("/rewards")
def get_rewards():
  round_points = matrix.round_points
  matrix.round_points = 0
  return json.dumps(round_points)


@app.route("/menu")
def get_menu():
  matrix.round_points = 0
  return json.dumps(matrix.round_points)





@app.route("/")
def get_ready():
  return "ready"


if __name__ == "__main__":
  app.run(host="0.0.0.0")
