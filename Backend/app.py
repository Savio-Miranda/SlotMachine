from flask import Flask
import send_pattern

app = Flask(__name__)

@app.route("/")
def index():
    p = send_pattern.pattern(5, 3)
    return p
