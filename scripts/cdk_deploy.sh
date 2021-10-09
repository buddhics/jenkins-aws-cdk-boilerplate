#!/bin/bash

while getopts ":r:e:" opt; do
  case $opt in
    r) region="$OPTARG"
    ;;
    e) env="$OPTARG"
    ;;
    \?) echo "Invalid option -$OPTARG" >&2
    exit 1
    ;;
  esac

  case $OPTARG in
    -*) echo "Option $opt needs a valid argument"
    exit 1
    ;;
  esac
done

$aws_identity_data = aws sts get-caller-identity
$aws_account_id = $(jq -r 'Account' <<< ${$aws_identity_data})
cdk bootstrap aws://$aws_account_id/$r -c envName=$e
cdk deploy -c envName=$e --require-approval=never
