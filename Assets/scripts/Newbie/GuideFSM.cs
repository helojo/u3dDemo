namespace Newbie
{
    using System;

    public class GuideFSM
    {
        public GuideStatus actived_status;
        public Func<bool> condition_reached;
        public Action transition_awake;
        public Action transition_cancel;
        public Action<List<GameObject>> transition_generate;
        public Action<GameObject> transition_response;
        public Action transition_timeout;
        public string valid_tag = string.Empty;

        public void OnReceive(GuideStatus status, object vari)
        {
            if (this.actived_status != status)
            {
                this.actived_status = status;
                switch (this.actived_status)
                {
                    case GuideStatus.Awake:
                        if (this.transition_awake != null)
                        {
                            this.transition_awake();
                        }
                        break;

                    case GuideStatus.UIGenerate:
                    {
                        UIGenerateParameter parameter = vari as UIGenerateParameter;
                        if (((parameter == null) || (this.transition_generate == null)) || !(parameter.tag == this.valid_tag))
                        {
                            if (parameter != null)
                            {
                                if (this.transition_generate != null)
                                {
                                    this.transition_generate(parameter.focuses);
                                }
                            }
                            else if (this.transition_cancel != null)
                            {
                                this.transition_cancel();
                            }
                            break;
                        }
                        this.transition_generate(parameter.focuses);
                        break;
                    }
                    case GuideStatus.UIResponse:
                    {
                        UIResponseParameter parameter2 = vari as UIResponseParameter;
                        if (((parameter2 == null) || (this.transition_response == null)) || !(parameter2.tag == this.valid_tag))
                        {
                            if (this.transition_response != null)
                            {
                                this.transition_response(null);
                            }
                            break;
                        }
                        this.transition_response(parameter2.ui_object);
                        break;
                    }
                    case GuideStatus.UICancel:
                        if (this.transition_cancel != null)
                        {
                            this.transition_cancel();
                        }
                        break;

                    case GuideStatus.Timeout:
                        if (this.transition_timeout != null)
                        {
                            this.transition_timeout();
                        }
                        break;
                }
            }
        }

        public bool WaittingAwake()
        {
            return (GuideStatus.Prepare == this.actived_status);
        }

        public bool WaittingGenerate()
        {
            return (GuideStatus.Awake == this.actived_status);
        }

        public bool WaittingResponse()
        {
            return (GuideStatus.UIGenerate == this.actived_status);
        }

        public enum GuideStatus
        {
            Prepare,
            Awake,
            UIGenerate,
            UIResponse,
            UICancel,
            Timeout
        }
    }
}

