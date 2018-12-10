package com.apple.apiforji.model;

public class ResultModel {
    private String returnCode;
    private String returnErrorSolution;
    private String returnMessage;
    private ReturnValue returnValue;

    public String getReturnCode() {
        return returnCode;
    }

    public void setReturnCode(String returnCode) {
        this.returnCode = returnCode;
    }

    public String getReturnErrorSolution() {
        return returnErrorSolution;
    }

    public void setReturnErrorSolution(String returnErrorSolution) {
        this.returnErrorSolution = returnErrorSolution;
    }

    public String getReturnMessage() {
        return returnMessage;
    }

    public void setReturnMessage(String returnMessage) {
        this.returnMessage = returnMessage;
    }

    public ReturnValue getReturnValue() {
        return returnValue;
    }

    public void setReturnValue(ReturnValue returnValue) {
        this.returnValue = returnValue;
    }
}
