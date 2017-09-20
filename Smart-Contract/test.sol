pragma solidity ^0.4.15;

contract Test {
    int _multiplier;

    function Test (int multiplier) {
        _multiplier = multiplier;
    }

    function multiply (int val) returns (int d) {
        return val * _multiplier;
    }

}   