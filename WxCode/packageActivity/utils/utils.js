function random() {
  var min = 1;
  var max = 5;
  return Math.floor(Math.random() * (max - min)) + min;
};

function nonRepeating(num) {
  var min = num;
  var max = 5;
  if (num >= max) {
    return 4;
  } else {
    return Math.floor(Math.random() * (max - min)) + min;
  }
};

module.exports = {
  random: random,
  nonRepeating: nonRepeating
}