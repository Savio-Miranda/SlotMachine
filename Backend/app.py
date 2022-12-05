from flask import Flask
import send_pattern
from send_matrix import Matrix


matrix = Matrix(5, 3, 11)
app = Flask(__name__)


@app.route("/")
def index():
    p = send_pattern.pattern(5, 3)
    return p


@app.route("/matrix")
def get_matrix():
    m = matrix.structure()
    return m


@app.route("/rewards")
def get_rewards():
    r = matrix.rewards()
    return r